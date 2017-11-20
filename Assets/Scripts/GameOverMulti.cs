using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameOverMulti : NetworkBehaviour
{
    [SyncVar]
    public float timer = 10f;

    public Text text;

    public GameObject ScoresPanel;

    private static GameOverMulti instance;
    public static GameOverMulti Instance { get { return instance; } }

    private void Start()
    {
        instance = this;

        if (isServer)
            PlayerNetworkSetup.Instance.CmdAddScorePanel();
    }

    [Command]
    public void CmdAddScorePanel()
    {
        foreach (Player p in NetworkManagerHUD.Instance.playerList)
        {
            GameObject childObject = Instantiate(ScoresPanel) as GameObject;
            childObject.transform.Find("Score").GetComponent<Text>().text = p.score.ToString();
            childObject.transform.Find("Level").GetComponent<Text>().text = MultiGameController.currentStage.ToString();
            childObject.transform.Find("Name").GetComponent<Text>().text = p.playerName;

            NetworkServer.Spawn(childObject);
        }

        foreach (Player p in NetworkManagerHUD.Instance.playerList)
            RpcAddScorePanel(p);
    }

    [ClientRpc]
    public void RpcAddScorePanel(Player p)
    {
        GameObject.FindGameObjectWithTag("ScoresPanel").transform.Find("Score").GetComponent<Text>().text = p.score.ToString();
        GameObject.FindGameObjectWithTag("ScoresPanel").transform.Find("Level").GetComponent<Text>().text = MultiGameController.currentStage.ToString();
        GameObject.FindGameObjectWithTag("ScoresPanel").transform.Find("Name").GetComponent<Text>().text = p.playerName;
    }

    private void Update()
    {
        if (isServer && timer >= 0)
            timer -= Time.deltaTime;

        text.text = ((int)timer).ToString();

        //if (timer < 0 && isServer)
            //NetworkManager.singleton.ServerChangeScene("LobbyMatch");
    }
}
