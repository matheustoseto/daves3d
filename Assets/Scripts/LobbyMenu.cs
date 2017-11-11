using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyMenu : NetworkBehaviour
{
    public GameObject readyButton;
    public bool readyPlayerGame = false;

    private void Update()
    {
        
    }

    public void BackLobbyMenu()
    {
        NetworkManagerHUD.Instance.CustomStopServer();
    }

    public void ReadyGame()
    {
        if (!readyPlayerGame)
        {
            readyPlayerGame = true;
            readyButton.GetComponent<Image>().color = Color.green;
        } else {
            readyPlayerGame = false;
            readyButton.GetComponent<Image>().color = Color.red;
        }
        
    }
}
