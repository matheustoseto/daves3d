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
   
    private Vector3 autoLocalScale = new Vector3(1, 1, 1);
    public GridLayoutGroup Grid;
    public GameObject playerLobby;

    private static LobbyController instance;
    public static LobbyController Instance { get { return instance; } }

    void Start()
    {
        instance = this;      
    }

    [Command]
    public void CmdAddPlayer()
    {
        AddPlayer();

        int i = 0;
        foreach (Player p in NetworkManagerHUD.Instance.playerList)
        {
            i++;
            RpcAddPlayer(p, i);        
        }       
    }

    [ClientRpc]
    public void RpcAddPlayer(Player p, int listLength)
    {
        if (GameObject.Find("LobbyPanel") != null && !isServer && listLength > GameObject.Find("LobbyPanel").transform.childCount)
        {
            GameObject childObject = Instantiate(playerLobby) as GameObject;
            childObject.transform.Find("PlayerName").GetComponent<InputField>().text = p.playerName;
            childObject.transform.Find("Color").GetComponent<Image>().color = p.color;
            childObject.GetComponent<LobbyMenu>().playerIndex = p.index;

            childObject.transform.SetParent(Grid.transform);
            childObject.transform.localScale = autoLocalScale;
            childObject.transform.localPosition = Vector3.zero;
        }
    }

    public void AddPlayer()
    {
        if (SceneManager.GetActiveScene().name.Equals("LobbyMatch") && isServer)
        {
            Player player = new Player();
            player.playerName = "Player" + (NetworkManagerHUD.Instance.playerList.Count <= 0 ? "1" :((int)NetworkManagerHUD.Instance.playerList.Count + 1).ToString());

            indexColor++;
            player.color = GetPlayerColor(indexColor);
            player.isRenderInPlayer = false;
            player.index = NetworkManagerHUD.Instance.playerList.Count;

            NetworkManagerHUD.Instance.playerList.Add(player);

            if (GameObject.Find("LobbyPanel") != null)
            {
                GameObject childObject = Instantiate(playerLobby) as GameObject;
                childObject.transform.Find("PlayerName").GetComponent<InputField>().text = player.playerName;
                childObject.transform.Find("Color").GetComponent<Image>().color = player.color;
                childObject.GetComponent<LobbyMenu>().playerIndex = player.index;

                childObject.transform.SetParent(Grid.transform);
                childObject.transform.localScale = autoLocalScale;
                childObject.transform.localPosition = Vector3.zero;
            }
        }
    }

    public Player GetPlayer(int index)
    {
        Player player = (from item in NetworkManagerHUD.Instance.playerList
                         where item.index == index
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

    public void ChangePlayerColor(int color, int index)
    {
        Player player = GetPlayer(index);
        player.color = GetPlayerColor(color);
        UpdatePlayer(player);
    }

    public void ReadyGame()
    {
        if (!isServer)
            return;

        foreach (Player p in NetworkManagerHUD.Instance.playerList)
        {
            if (!p.playerReady)
            {
                break;
            }
            SceneManager.LoadScene("Multi_fase_1", LoadSceneMode.Single);
        }
    }
}
