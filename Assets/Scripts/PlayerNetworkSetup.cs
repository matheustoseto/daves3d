using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField] Camera playerCam;
    [SerializeField] AudioListener playerAudio;

    private static PlayerNetworkSetup instance;
    public static PlayerNetworkSetup Instance { get { return instance; } }

    public bool isViewer = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (isLocalPlayer)
        {
            instance = this;
            CmdGameStart(NetworkManagerHUD.Instance.playerName);
        }
               
    }

    [Command]
    public void CmdGameStart(string playerName)
    {
        RpcGameStart(playerName, NetworkManagerHUD.Instance.gameStart);
    }

    [ClientRpc]
    public void RpcGameStart(string playerName, bool isReady)
    {
        if (playerName.Equals(NetworkManagerHUD.Instance.playerName))
        {
            if (isReady)
            {
                isViewer = true;
                SceneManager.LoadScene("Multi_fase_1", LoadSceneMode.Single);
            }
            else
            {
                CmdAddPlayer(NetworkManagerHUD.Instance.playerName);
            }
        }
    }

    [Command]
    public void CmdSendText(string playerName, string txt)
    {
        LobbyController.Instance.CmdAddText(playerName, txt);
    }

    [Command]
    public void CmdAddPlayer(string playerName)
    {
        LobbyController.Instance.CmdAddPlayer(playerName);
    }

    [Command]
    public void CmdReadyGame(string playerName)
    {
        LobbyController.Instance.CmdSetReadyGame(playerName);
        LobbyController.Instance.CmdReadyGame();
    }

    [Command]
    public void CmdChangeColor(string playerName)
    {
        LobbyController.Instance.CmdChangeColor(playerName);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (!SceneManager.GetActiveScene().name.Equals("LobbyMatch") && isLocalPlayer)
        {
            if (!isViewer)
            {
                GameObject.Find("Scene Camera").SetActive(false);

                GetComponent<CharacterController>().enabled = true;
                GetComponent<PlayerController>().enabled = true;
                GetComponent<ShowItens>().enabled = true;
                GetComponent<MultiGameController>().enabled = true;
                playerCam.enabled = true;
                playerAudio.enabled = true;
            } else
            {
                CmdDeletePlayer(gameObject);
            }         
        }
    }

    [Command]
    public void CmdDeletePlayer(GameObject player)
    {
        Destroy(player);
        RpcDeletePlayer(player);
    }

    [ClientRpc]
    public void RpcDeletePlayer(GameObject player)
    {
        Destroy(player);
    }
}
