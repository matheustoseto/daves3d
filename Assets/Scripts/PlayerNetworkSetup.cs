using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField] Camera playerCam;
    [SerializeField] AudioListener playerAudio;

    void Start () {
	    if(isLocalPlayer)
        {
            GameObject.Find("Scene Camera").SetActive(false);

            GetComponent<CharacterController>().enabled = true;
           // GetComponent<PlayerController>().enabled = true;
            GetComponent<ShowItens>().enabled = true;
            playerCam.enabled = true;
            playerAudio.enabled = true;
        }
	}
	
}
