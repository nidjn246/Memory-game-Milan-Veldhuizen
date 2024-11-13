using System.Runtime.CompilerServices;
using UnityEngine;


public enum CardStatus
{
    show_back = 0,
    show_front,
    rotating_to_back,
    rotating_to_front
}


public class Card : MonoBehaviour
{
    [SerializeField] float turnTargetTime;
    [SerializeField] float turnTimer;
    [SerializeField] private CardStatus status;
    [SerializeField] Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;
     SpriteRenderer frontRenderer;
     SpriteRenderer backRenderer;
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

    private void Awake()
    {
        status = CardStatus.show_back;
        GetFrontAndBackSpriteRenderers();
    }

    private void OnMouseUp()
    {
        if (status == CardStatus.show_back)
        {
            TurnToFront();
        }

        if (status == CardStatus.show_front)
        {
            TurnToBack();
        }

    }

    public void TurnToFront()
    {
        status = CardStatus.rotating_to_front;
        turnTimer = 0f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0);
    }

    public void TurnToBack()
    {
        status = CardStatus.rotating_to_back;
        turnTimer = 0f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

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
}
