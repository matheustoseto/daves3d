using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	void LoadStartGame()
    {
        SceneManager.LoadScene("Single_fase_1", LoadSceneMode.Single);
    }

    void LoadMultiPlayer()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
