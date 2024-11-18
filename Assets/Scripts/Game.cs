//using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Game : MonoBehaviour
{
    public enum GameStatus
    {
        waiting_on_first_card,
        waiting_on_second_card,
        match_found,
        no_match_found
    }

    [SerializeField] int rows = 3;
    [SerializeField] int columns = 4;
    [SerializeField] int totalpairs;
    [SerializeField] string frontsideFolder = "Sprites/Frontsides";
    [SerializeField] string backsideFolder = "Sprites/Backsides";
    [SerializeField] Sprite[] FrontSprites;
    [SerializeField] Sprite[] BackSprites;
    [SerializeField] Sprite selectedBackSprite;
    [SerializeField] List<Sprite> selectedFrontSprites;
    [SerializeField] GameObject cardPrefab;
    Stack<GameObject> stackOfCards;
    GameObject[,] placedCards;
    [SerializeField] Transform fieldAnchor;
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] GameStatus status;
    [SerializeField]  GameObject[] selectedCards;
    private float timeoutTimer;
    [SerializeField] float timeoutTarget;

    void Start()
    {
        placedCards = new GameObject[columns, rows];
        MakeCards();
        DistributeCards();
        selectedCards = new GameObject[2];
        status = GameStatus.waiting_on_first_card;
    }

    void Update()
    {
        if (status == GameStatus.match_found || status == GameStatus.no_match_found)
        {
            RotateBackOrRemovePair();
        }
    }

    void MakeCards()
    {
        CalculateAmountOfPairs();
        LoadSprites();
        SelectFrontSprites();
        SelectBackSprites();
        ConstructCards();
    }

    private void DistributeCards()
    {
        GameObject[,] placedCards = new GameObject[columns, rows];
        ShuffleCards();
        PlaceCardsOnField();
    }

    void CalculateAmountOfPairs()
    {
        if (rows * columns % 2 == 0)
        {
            totalpairs = rows * columns / 2;
        }
        else 
        {
            Debug.LogError("werkt niret errorr");
        }
    }

    private void LoadSprites()
    {
        FrontSprites = Resources.LoadAll<Sprite>(frontsideFolder);
        BackSprites = Resources.LoadAll<Sprite>(backsideFolder);
    }

    private void SelectFrontSprites()
    {
        if (FrontSprites.Length < totalpairs)
        {
            Debug.LogError("te wienig plaatjes om" + totalpairs + " paren te maken");
        }

        selectedFrontSprites = new List<Sprite>();

        while (selectedFrontSprites.Count < totalpairs)
        {
            int rnd = Random.Range( 0, FrontSprites.Length);
            if (selectedFrontSprites.Contains(FrontSprites[rnd]) == false) 
            {
                selectedFrontSprites.Add(FrontSprites[rnd]);
            }
        }
    }

    private void SelectBackSprites()
    {
        if (BackSprites.Length > 0)
        {
            int rnd = Random.Range(0, BackSprites.Length);
            selectedBackSprite = BackSprites[rnd];
        }
        else
        {
            Debug.LogError("er zijn geen achetkant plaatjes");
        }
    }

    private void ConstructCards()
    {
        stackOfCards = new Stack<GameObject>();

        GameObject parent = new GameObject();
        parent.name = "Cards";

        foreach (Sprite sprite in selectedFrontSprites)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(cardPrefab);
                Card cardscript = go.GetComponent<Card>();

                cardscript.SetBack(selectedBackSprite);
                cardscript.SetFront(sprite);

                go.name = sprite.name;
                go.transform.parent = parent.transform;

                stackOfCards.Push(go);
            }
        }
    }

    private void ShuffleCards()
    {
        Debug.Log(placedCards.GetLength(0) + " " + placedCards.GetLength(1));
        while (stackOfCards.Count > 0)
        {
            
            int randX = Random.Range(0, columns);
            int randY = Random.Range(0, rows);

            if (placedCards[randX, randY] == null)
            {

                placedCards[randX, randY] = stackOfCards.Pop();
            }
        }
    }

    private void PlaceCardsOnField()
    {
        for (int y = 0; y < rows; y++)
        {
            for(int x = 0; x < columns; x++)
            {
                GameObject card = placedCards[x, y];
                
                Card cardscript = card.GetComponent<Card>();

                Vector2 cardsize = cardscript.GetBackSize();

                float xpos = fieldAnchor.transform.position.x + (x * (cardsize.x + offsetX));
                float ypos = fieldAnchor.transform.position.y - (y * (cardsize.y + offsetY));

                print(card.transform.lossyScale.x);

                placedCards[x, y].transform.position = new Vector3(xpos, ypos, 0f);
            }
        }
    }

    public void SelectCard(GameObject card)
    {
        if (status == GameStatus.waiting_on_first_card) 
        {
            selectedCards[0] = card;
            status = GameStatus.waiting_on_second_card;
        }
        else if (status == GameStatus.waiting_on_second_card)
        {
            selectedCards[1] = card;
            CheckForMatchingPair();
        }
    }

    private void CheckForMatchingPair()
    {
        timeoutTimer = 0f;
        if (selectedCards[0].name == selectedCards[1].name)
        {
            status = GameStatus.match_found;
        }
        else 
        {
            status = GameStatus.no_match_found;
        }
    }

    private void RotateBackOrRemovePair() 
    {
        timeoutTimer += Time.deltaTime;

        if (timeoutTimer >= timeoutTarget)
        {
            if (status == GameStatus.match_found)
            {
                selectedCards[0].SetActive(false);
                selectedCards[1].SetActive(false);
            }
        if (status == GameStatus.no_match_found) 
        {
            selectedCards[0].GetComponent<Card>().TurnToBack();
            selectedCards[1].GetComponent<Card>().TurnToBack();

        }
            selectedCards[0] = null;
            selectedCards[1] = null;
            
            status = GameStatus.waiting_on_first_card;
        }

    }

    public bool AllowedToSelectCard(Card card)
    {
        if (selectedCards[0] == null)
        {
            return true;
        }

        if (selectedCards[1] == null)
        {
            if (selectedCards[0] != card.gameObject)
            {
                return true;
            }
        }
            return false;
    }
}
