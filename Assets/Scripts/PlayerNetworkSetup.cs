using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField] Camera playerCam;
    [SerializeField] AudioListener playerAudio;

    private static PlayerNetworkSetup instance;
    public static PlayerNetworkSetup Instance { get { return instance; } }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (isLocalPlayer)
        {
            instance = this;
            CmdAddPlayer(NetworkManagerHUD.Instance.playerName);
        }       
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
            GameObject.Find("Scene Camera").SetActive(false);

            GetComponent<CharacterController>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerSync>().enabled = true;
            GetComponent<PlayerSyncRotation>().enabled = true;
            GetComponent<ShowItens>().enabled = true;
            playerCam.enabled = true;
            playerAudio.enabled = true;
        }
    }

}
