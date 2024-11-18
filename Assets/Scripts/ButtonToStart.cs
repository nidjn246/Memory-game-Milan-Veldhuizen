using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToStart : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("Memory game");
    }

    private void OnMouseDown()
    {
        NextScene();
    }
}
