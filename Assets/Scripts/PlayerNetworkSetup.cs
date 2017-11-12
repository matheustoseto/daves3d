using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField] Camera playerCam;
    [SerializeField] AudioListener playerAudio;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (isLocalPlayer)
            CmdAddPlayer();
    }

    [Command]
    public void CmdAddPlayer()
    {
        LobbyController.Instance.CmdAddPlayer();
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
