using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSync : NetworkBehaviour {


    [SyncVar]
    private Vector3 syncPos;

    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    float lerpRate = 20;

    private Vector3 lastPos;
    private float threshold = 1f;
    
    void Update ()
    {
        TransmitPosition();
        LerpPosition();
	}	
	
	void LerpPosition ()
    {
	    if(!isLocalPlayer)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
	}

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [Client]
    void TransmitPosition()
    {
        //Verifica se o o jogador andou mais que a variavel Threshold
        if(isLocalPlayer && Vector3.Distance(playerTransform.position, lastPos) < threshold)
        {
            CmdProvidePositionToServer(playerTransform.position);
            lastPos = playerTransform.position;
        }        
    }
}
