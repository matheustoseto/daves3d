﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class NetworkManagerHUD : NetworkManager
{
    private NetworkManager manager;
    public GameObject canvasMenu;

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
}