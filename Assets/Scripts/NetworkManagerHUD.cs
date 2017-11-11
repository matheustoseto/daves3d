using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player
{
    public string name;
    public Color color;
}

public class NetworkManagerHUD : NetworkManager
{
    private NetworkManager manager;
    [SerializeField] public bool showGUI = true;
    [SerializeField] public int offsetX;
    [SerializeField] public int offsetY;
    [SerializeField] public GameObject canvasMenu;
    [SerializeField] public GameObject playerLobby;
    [SerializeField] public List<Player> playerList = new List<Player>();

    bool showServer = false;

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
        HideCanvasMenu();
        manager.StartHost();
    }

    public void CustomJoinGame()
    {
        SetIpAddress();
        SetPort();
        HideCanvasMenu();
        manager.StartClient();
    }

    public void SetIpAddress()
    {
        string ip = GameObject.Find("InputIpAddress").transform.FindChild("Text").GetComponent<Text>().text;
        manager.networkAddress = ip;
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
    }

    public Color GetPlayerColor(int index)
    {
        switch (index)
        {
            case 1:
                {
                    return Color.red;
                }
            case 2:
                {
                    return Color.green;
                }
            case 3:
                {
                    return Color.black;
                }
            case 4:
                {
                    return Color.blue;
                }
            default:
                {
                    return Color.red;
                }
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name.Equals("LobbyMatch"))
        {
            Player player = new Player();
            player.name = "Player" + (playerList.Count <= 0 ? "1" : playerList.Count.ToString());
            int index = playerList.Count <= 0 ? 1 : playerList.Count;
            player.color = GetPlayerColor(index);

            playerList.Add(player);

            playerLobby.transform.FindChild("InputPlayeName").transform.FindChild("Text").GetComponent<Text>().text = player.name;
            playerLobby.transform.FindChild("Color").GetComponent<Image>().color = player.color;

            GameObject.Find("LobbyPanel").transform.parent = playerLobby.transform;
        }
            
    }

    void OnGUI()
    {
        if (!showGUI)
            return;

        int xpos = 10 + offsetX;
        int ypos = 40 + offsetY;
        int spacing = 24;

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

        if (NetworkClient.active && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
            {
                ClientScene.Ready(manager.client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }

        if (NetworkServer.active || NetworkClient.active)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
            {
                CustomStopServer();
            }
            ypos += spacing;
        }
    }
}
