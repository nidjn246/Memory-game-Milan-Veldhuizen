using System.Runtime.CompilerServices;
using UnityEngine;


//De status van het kaartje
public enum CardStatus
{
    show_back = 0,
    show_front,
    rotating_to_back,
    rotating_to_front
}


public class Card : MonoBehaviour
{
    //Alle variabelen die nodig zijn in dit script
    [SerializeField] float turnTargetTime;
    [SerializeField] float turnTimer;
    [SerializeField] private CardStatus status;
    [SerializeField] Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;
     public SpriteRenderer frontRenderer;
     public SpriteRenderer backRenderer;
    Game game;

    //wanneer de game begint word dit uitgevoert
    public void Awake()
    {
        //de status word show_back en hij pakt alle sprites voor de kaartjes
        status = CardStatus.show_back;
        GetFrontAndBackSpriteRenderers();
        game = FindObjectOfType<Game>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (status == CardStatus.rotating_to_front || status == CardStatus.rotating_to_back)
        {
            turnTimer += Time.deltaTime;
            float percentage = turnTimer/turnTargetTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentage);
            if (percentage >= 1f)
            {
                if (status == CardStatus.rotating_to_back)
                {
                    status = CardStatus.show_back;
                }
                else if (status == CardStatus.rotating_to_front) 
                {
                    status = CardStatus.show_front;

                }

            }
        }
    }

    private void OnMouseUp()
    {
        if (game.AllowedToSelectCard(this) == true)
        {
            if (status == CardStatus.show_back)
            {
                game.SelectCard(gameObject);
                TurnToFront();
            }

            if (status == CardStatus.show_front)
            {
                TurnToBack();
            }
        }
    }
    //Wanneer deze functie word gecalled draait het kaartje naar de voorkant
    public void TurnToFront()
    {
        //maak de status dat het kaartje aan het draaien is naar de voorkant
        status = CardStatus.rotating_to_front;
        turnTimer = 0f;
        //zet de transform rotation in de startrotation
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0);
    }

    //Wanneer deze functie word gecalled draait het kaartje naar de achterkant
    public void TurnToBack()
    {
        status = CardStatus.rotating_to_back;
        turnTimer = 0f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    //deze functie pakt alle sprites en renderd ze in de front en back renderer
    private void GetFrontAndBackSpriteRenderers()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "Front")
            {
                frontRenderer = t.GetComponent<SpriteRenderer>();
            }

            else if(t.name == "Back")
            {
                backRenderer = t.GetComponent<SpriteRenderer>();
            }
        }
    }

    //hier zet de renderer de sprite op de voorkant van het nieuwe gameobject
    public void SetFront(Sprite sprite)
    {
        if (frontRenderer != null)
        {
            frontRenderer.sprite = sprite;
        }
    }

    //hier zet de renderer de sprite op de achterkant van het nieuwe gameobject
    public void SetBack(Sprite sprite)
    {
        if (backRenderer != null)
        {
            backRenderer.sprite = sprite;
        }

    }

    //hier kijkt de code hoe groot de voorkant het kaartje is dat is gemaakt
    public Vector2 GetFrontSize()
    {
        if(frontRenderer == null)
        {
            Debug.LogError("er is geen frontrenderer");
        }
        return frontRenderer.bounds.size;
    }

    //hier kijkt de code hoe groot de achterkant het kaartje is dat is gemaakt
    public Vector2 GetBackSize() 
    {
        if (backRenderer == null)
        {
            Debug.LogError("Er is geen backrenderer gevonden");
        }
        return backRenderer.bounds.size;
    }
}
