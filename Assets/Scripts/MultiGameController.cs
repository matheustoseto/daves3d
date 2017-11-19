﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class MultiGameController : NetworkBehaviour
{
    public GameObject player;
    public GameObject door;
    public GameObject portal;
    public Material materialOpenDoor;

    public int Lifes = 3;
    public bool removeLife = false;
    public Texture2D davesLife;
    public Texture2D openDoorTexture;
    public Texture2D gunTexture;
    public Texture2D jackpackTexture;

    public bool openDoor = false;

    private string fmt = "00000";
    public int score = 0;
    public Font font;

    public static int currentStage;

    public Texture2D jackPackBar_1;
    public Texture2D jackPackBar_2;

    public GameObject ScoresPanel;

    private static MultiGameController instance = null;

    public static MultiGameController Instance
    {
        get { return instance; }
    }

    void Start()
    {
        currentStage = 1;
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (GameObject.FindGameObjectWithTag("Door"))
        {
            door = GameObject.FindGameObjectWithTag("Door");
            portal = door.transform.GetChild(1).gameObject;
            portal.SetActive(false);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name.Equals("Menu"))
            Destroy(gameObject);

        if (GameObject.FindGameObjectWithTag("Door"))
        {
            door = GameObject.FindGameObjectWithTag("Door");
            portal = door.transform.GetChild(1).gameObject;
            portal.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name.Equals("Single_GameOver") && isServer)
        {
            foreach (Player p in NetworkManagerHUD.Instance.playerList)
            {
                GameObject childObject = Instantiate(ScoresPanel) as GameObject;
                childObject.transform.Find("Score").GetComponent<Text>().text = p.score.ToString();
                childObject.transform.Find("Level").GetComponent<Text>().text = currentStage.ToString();
                childObject.transform.Find("Name").GetComponent<Text>().text = p.playerName;

                NetworkServer.Spawn(childObject);
            }

            foreach (Player p in NetworkManagerHUD.Instance.playerList)
                RpcAddScorePanel(p);

            gameObject.SetActive(false);
        }
        else
        {
            openDoor = false;
            currentStage++;
        }
    }

    [Command]
    public void CmdLoadGameOverScene()
    {
        SceneManager.LoadScene("Multi_GameOver", LoadSceneMode.Single);
        RpcLoadGameOverScene();
    }

    [ClientRpc]
    public void RpcLoadGameOverScene()
    {
        SceneManager.LoadScene("Multi_GameOver", LoadSceneMode.Single);
    }

    [ClientRpc]
    public void RpcAddScorePanel(Player p)
    {
        GameObject.FindGameObjectsWithTag("ScoresPanel")[p.index].transform.Find("Score").GetComponent<Text>().text = p.score.ToString();
        GameObject.FindGameObjectsWithTag("ScoresPanel")[p.index].transform.Find("Level").GetComponent<Text>().text = currentStage.ToString();
        GameObject.FindGameObjectsWithTag("ScoresPanel")[p.index].transform.Find("Name").GetComponent<Text>().text = p.playerName;

        gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        door.GetComponent<MeshRenderer>().material = materialOpenDoor;
        openDoor = true;
    }

    void OnGUI()
    {
        int space = 20;
        GUI.skin.font = font;
        GUI.skin.label.fontSize = 40;

        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        if (Lifes > 0)
        {
            GUI.Label(new Rect(15, 15, 300, 50), score.ToString(fmt));
            for (int i = 0; i < Lifes; i++)
            {
                GUI.Label(new Rect(Screen.width - 200 + (30 * i) + space, 15, 64, 32), davesLife);
                space += 20;
            }
            GUI.Label(new Rect((Screen.width / 2) - (150 / 2), 15, 300, 50), "Level : " + currentStage);

            if (player.GetComponent<PlayerController>().hasPistol)
                GUI.Label(new Rect(15, Screen.height - 50, 300, 50), gunTexture);

            if (openDoor)
                GUI.Label(new Rect(Screen.width - 271, Screen.height - 45, 400, 100), openDoorTexture);

            if (player.GetComponent<PlayerController>().hasJetPack)
            {
                GUI.Label(new Rect((Screen.width / 2) - (150 / 2), Screen.height - 50, 300, 50), jackpackTexture);
                GUI.DrawTexture(new Rect((Screen.width / 2) - (340 / 2), Screen.height - 20, 300 / 50 * player.GetComponent<PlayerController>().maxJetpack, 15), jackPackBar_2);
            }
                
        }

        GUI.EndGroup();
    }

    public Player GetPlayer(string playerName)
    {
        Player player = (from item in NetworkManagerHUD.Instance.playerList
                         where item.playerName == playerName
                         select item).FirstOrDefault();

        return player;
    }

    public void UpdatePlayer(Player p)
    {
        var obj = NetworkManagerHUD.Instance.playerList.FirstOrDefault(x => x.index == p.index);
        if (obj != null) obj = p;
    }

    [Command]
    public void CmdAddScore(string playerName, int score)
    {
        Player player = GetPlayer(playerName);

        player.score = score;

        UpdatePlayer(player);
    }

    public void AddScore(int scr)
    {
        score += scr;

        CmdAddScore(NetworkManagerHUD.Instance.playerName, scr);
    }

    public void RemoveLife()
    {
        if (!removeLife)
        {
            removeLife = true;
            Lifes--;
            player.transform.position = player.GetComponent<PlayerController>().startPoint;

            if (Lifes <= 0)
                CmdLoadGameOverScene();
            removeLife = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PickUp"))
        {
            AddScoreAndDestroy(other.gameObject);
        }
        if (other.gameObject.tag.Equals("Cup"))
        {
            AddScoreAndDestroy(other.gameObject);
            OpenDoor();
        }
        if (openDoor && other.gameObject.tag.Equals("Door"))
        {
            SceneManager.LoadScene("Single_fase_" + (currentStage + 1), LoadSceneMode.Single);
        }
        if (other.gameObject.tag.Equals("GroundDie"))
        {
            RemoveLife();
        }
    }

    void AddScoreAndDestroy(GameObject obj)
    {
        AddScore(obj.GetComponent<ScoreMulti>().points);
        Destroy(obj);
    }
}
