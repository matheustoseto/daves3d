using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSync : NetworkBehaviour {


    [SyncVar]
    public Vector3 syncPos;

    public Transform playerTransform;
    public float lerpRate = 15;
    public Vector3 lastPos;
    public float threshold = 1f;
    public bool ready = false;

    private void Start()
    {
        Vector3 startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        playerTransform.position = startPoint;
        syncPos = startPoint;
        lastPos = startPoint;
    }

    void Update ()
    {
        if (!ready)
        {
            playerTransform.position = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
            ready = true;
        }

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
