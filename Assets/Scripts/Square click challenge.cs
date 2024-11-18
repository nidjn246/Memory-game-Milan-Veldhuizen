using UnityEngine;

public class Squareclickchallenge : MonoBehaviour
{
    void Start()
    {
        OnMouseUp();
    }

    private void OnMouseUp()
    {
       float RandomY = Random.Range(-4f, 4f);
      float RandomX = Random.Range(-8f, 8f);
        transform.position = new Vector3(RandomX, RandomY, transform.position.z);

    }
}
