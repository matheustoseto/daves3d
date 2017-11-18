using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class LobbyController : NetworkBehaviour
{
    [SyncVar]
    public int indexColor = 0;

    public GameObject playerLobby;

    private static LobbyController instance;
    public static LobbyController Instance { get { return instance; } }

    void Start()
    {
        instance = this;      
    }

    [Command]
    public void CmdAddPlayer(string playerName)
    {
        AddPlayer(playerName);

        int i = 0;
        foreach (Player p in NetworkManagerHUD.Instance.playerList)
        {
            i++;
            if (GameObject.Find("LobbyPanel") != null && i > GameObject.Find("LobbyPanel").transform.childCount)
            {
                GameObject childObject = Instantiate(playerLobby) as GameObject;
                childObject.transform.Find("PlayerName").GetComponent<InputField>().text = p.playerName;
                childObject.transform.Find("Color").GetComponent<Image>().color = p.color;

                NetworkServer.Spawn(childObject);  
            }
        }

        foreach (Player p in NetworkManagerHUD.Instance.playerList)
            RpcAddPlayer(p);
    }

    [ClientRpc]
    public void RpcAddPlayer(Player p)
    {
        GameObject.FindGameObjectsWithTag("PlayerLobby")[p.index].transform.Find("PlayerName").GetComponent<InputField>().text = p.playerName;
        GameObject.FindGameObjectsWithTag("PlayerLobby")[p.index].transform.Find("Color").GetComponent<Image>().color = p.color;
    }

    public void AddPlayer(string playerName)
    {
        if (SceneManager.GetActiveScene().name.Equals("LobbyMatch") && isServer)
        {
            Player player = new Player();
            player.playerName = playerName;

            indexColor++;
            player.color = GetPlayerColor(indexColor);
            player.index = NetworkManagerHUD.Instance.playerList.Count;

            NetworkManagerHUD.Instance.playerList.Add(player);
        }
    }

    public void BackLobbyMenu()
    {
        NetworkManagerHUD.Instance.CustomStopServer();
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

    public Color GetPlayerColor(int index)
    {
        for (int i = 1; i <= 4; i++)
        {
            Player player = (from item in NetworkManagerHUD.Instance.playerList
                             where item.color == SwitchColor(index)
                             select item).FirstOrDefault();

            if (player == null)
                return SwitchColor(index);

            indexColor++;
            if (indexColor > 4)
                indexColor = 1;
        }
        if (index == indexColor)
            indexColor++;

        return SwitchColor(indexColor);
    }

    public Color SwitchColor(int index)
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

    public Color ChangePlayerColor(int color, string playerName)
    {
        Player player = GetPlayer(playerName);
        player.color = GetPlayerColor(color);
        UpdatePlayer(player);

        return player.color;
    }

    [Command]
    public void CmdReadyGame()
    {
        if (isServer && ReadyGame())
        {
            NetworkManagerHUD.Instance.gameStart = true;
            SceneManager.LoadScene("Multi_fase_1", LoadSceneMode.Single);
            RpcReadyGame();
        }    
    }

    [ClientRpc]
    public void RpcReadyGame()
    {
        SceneManager.LoadScene("Multi_fase_1", LoadSceneMode.Single);
    }

    public bool ReadyGame()
    {
        bool ready = true;

        foreach (Player p in NetworkManagerHUD.Instance.playerList)
        {
            if (!p.playerReady)
            {
                ready = false;
                break;
            }     
        }
        return ready;
    }

    [Command]
    public void CmdSetReadyGame(string playerName)
    {
        bool ready = false;
        if (isServer)
            ready = SetReadyGame(playerName);

        RpcSetReadyGame(playerName, ready);
    }

    [ClientRpc]
    public void RpcSetReadyGame(string playerName, bool ready)
    {
        if (GameObject.FindGameObjectsWithTag("PlayerLobby") != null && !isServer)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerLobby"))
            {
                if (playerName.Equals(go.transform.Find("PlayerName").transform.FindChild("Text").GetComponent<Text>().text))
                {
                    go.GetComponent<LobbyMenu>().readyButton.GetComponent<Image>().color = ready ? Color.green : Color.white;
                }
            }
        }
    }

    public bool SetReadyGame(string playerName)
    {
        Player player = GetPlayer(playerName);

        player.playerReady = !player.playerReady;

        UpdatePlayer(player);

        if (GameObject.FindGameObjectsWithTag("PlayerLobby") != null)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerLobby"))
            {
                if (playerName.Equals(go.transform.Find("PlayerName").transform.FindChild("Text").GetComponent<Text>().text))
                {
                    go.GetComponent<LobbyMenu>().readyButton.GetComponent<Image>().color = player.playerReady ? Color.green : Color.red;
                }
            }
        }

        return player.playerReady;
    }

    [Command]
    public void CmdChangeColor(string playerName)
    {
        Color color = Color.white;
        if (isServer)
            color = ChangeColor(playerName);

        RpcChangeColor(playerName, color);
    }

    [ClientRpc]
    public void RpcChangeColor(string playerName, Color color)
    {
        if (GameObject.FindGameObjectsWithTag("PlayerLobby") != null && !isServer)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerLobby"))
            {
                if (playerName.Equals(go.transform.Find("PlayerName").transform.FindChild("Text").GetComponent<Text>().text))
                {
                    go.GetComponent<LobbyMenu>().colorButton.GetComponent<Image>().color = color;
                }
            }
        }
    }

    public Color ChangeColor(string playerName)
    {
        indexColor++;
        if (indexColor > 4)
            indexColor = 1;

        Color color = ChangePlayerColor(indexColor, playerName);

        if (GameObject.FindGameObjectsWithTag("PlayerLobby") != null)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerLobby"))
            {
                if (playerName.Equals(go.transform.Find("PlayerName").transform.FindChild("Text").GetComponent<Text>().text))
                {
                    go.GetComponent<LobbyMenu>().colorButton.GetComponent<Image>().color = color;
                }
            }
        }

        return color;
    }
}
