using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    private void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width / 2) - (500 / 2), Screen.height / 2 + 70, 500, 50), "Clique aqui para voltar ao menu!"))
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
