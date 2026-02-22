using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void GoToMaingameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maingame");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        UnityEngine.Application.Quit();
    }
}
