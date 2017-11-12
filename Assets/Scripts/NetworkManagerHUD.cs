using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class NetworkManagerHUD : NetworkManager
{
    private NetworkManager manager;
    [SerializeField] public bool showGUI = true;
    [SerializeField] public int offsetX;
    [SerializeField] public int offsetY;
    [SerializeField] public GameObject canvasMenu;

    public string playerName = "";
    public List<Player> playerList = new List<Player>();

    private static NetworkManagerHUD instance;
    public static NetworkManagerHUD Instance { get { return instance; } }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        manager = GetComponent<NetworkManager>();
        instance = this;
    }

    void Update()
    {
        if (!showGUI)
            return;
    }

    public void CustomStartHost()
    {
        SetIpAddress();
        SetPort();
        SetPlayerName();
        HideCanvasMenu();       
        manager.StartHost();
    }

    public void CustomJoinGame()
    {
        SetIpAddress();
        SetPort();
        SetPlayerName();
        HideCanvasMenu();     
        manager.StartClient();
    }

    public void SetIpAddress()
    {
        string ip = GameObject.Find("InputIpAddress").transform.FindChild("Text").GetComponent<Text>().text;
        manager.networkAddress = ip;
    }

    public void SetPlayerName()
    {
        string name = GameObject.Find("InputPlayerName").transform.FindChild("Text").GetComponent<Text>().text;
        playerName = name;
    }

    public void SetPort()
    {
        manager.networkPort = 7777;
    }

    public void ShowCanvasMenu()
    {
        canvasMenu.SetActive(true);
    }

    public void HideCanvasMenu()
    {
        canvasMenu.SetActive(false);
    }

    public void BackMainMenu()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void CustomStopServer()
    {
        manager.StopHost();
        ShowCanvasMenu();
        Destroy(gameObject);
    }

    void OnGUI()
    {
        if (!showGUI)
            return;

        /*
        if (NetworkServer.active)
        {
            GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
            ypos += spacing;
        }
        if (NetworkClient.active)
        {
            GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
            ypos += spacing;
        }
        */
        if (NetworkClient.active && !ClientScene.ready)
        {
            /*
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
            {
                ClientScene.Ready(manager.client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
            */
        }

        if (NetworkServer.active || NetworkClient.active)
        {
            /*
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
            {
                CustomStopServer();
            }
            ypos += spacing;
            */
        }
    }
}
