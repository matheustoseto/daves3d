using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyMenu : NetworkBehaviour
{
    public GameObject readyButton;
    public GameObject colorButton;
    public InputField playerName;

    public int playerIndex;

    public void BackLobbyMenu()
    {
        NetworkManagerHUD.Instance.CustomStopServer();
    }

    public void ReadyGame()
    {
        Player player = LobbyController.Instance.GetPlayer(playerIndex);
        player.playerName = playerName.text;
        
        if (!player.playerReady)
        {
            player.playerReady = true;
            readyButton.GetComponent<Image>().color = Color.green;
        } else {
            player.playerReady = false;
            readyButton.GetComponent<Image>().color = Color.white;
        }
        LobbyController.Instance.UpdatePlayer(player);
        LobbyController.Instance.ReadyGame();
    }

    public void ChangeColor()
    {   
        LobbyController.Instance.indexColor++;
        if (LobbyController.Instance.indexColor > 4)
            LobbyController.Instance.indexColor = 1;

        LobbyController.Instance.ChangePlayerColor(LobbyController.Instance.indexColor, playerIndex);
        colorButton.GetComponent<Image>().color = LobbyController.Instance.GetPlayerColor(LobbyController.Instance.indexColor);
    }

}
