using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField] Camera playerCam;
    [SerializeField] AudioListener playerAudio;

    private static PlayerNetworkSetup instance;
    public static PlayerNetworkSetup Instance { get { return instance; } }

    public bool isViewer = false;

    public Material hatBlue;
    public Material hatRed;
    public Material hatGreen;
    public Material hatBlack;

    public TextMesh textPlayerName;
    public GameObject goPlayerName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (isLocalPlayer)
        {
            instance = this;
            CmdGameStart(NetworkManagerHUD.Instance.playerName);
        }
               
    }

    private void FixedUpdate()
    {
        goPlayerName.transform.localEulerAngles = transform.localEulerAngles;
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
        int i = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerLobby"))
        {
            Color color = go.GetComponent<LobbyMenu>().colorButton.GetComponent<Image>().color;
            GameObject.FindGameObjectsWithTag("Player")[i].transform.Find("Sphere").GetComponent<Renderer>().material = LobbyController.Instance.getMaterialByColor(color);
            i++;
        }
        RpcSetHatColor();
        LobbyController.Instance.CmdSetReadyGame(playerName);
        LobbyController.Instance.CmdReadyGame();
    }

    [ClientRpc]
    public void RpcSetHatColor()
    {
        int i = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerLobby"))
        {
            Color color = go.GetComponent<LobbyMenu>().colorButton.GetComponent<Image>().color;
            GameObject.FindGameObjectsWithTag("Player")[i].transform.Find("Sphere").GetComponent<Renderer>().material = LobbyController.Instance.getMaterialByColor(color);
            i++;
        }
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
                //goPlayerName.SetActive(true);
                playerCam.enabled = true;
                playerAudio.enabled = true;

                //if (isServer)
                //NetworkServer.SpawnObjects();

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
