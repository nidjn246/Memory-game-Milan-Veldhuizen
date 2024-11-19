using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToStopandStart : MonoBehaviour
{
    public void StopGame()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Memory game");
    }
}
