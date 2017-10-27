using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShowItens : NetworkBehaviour
{

    public GameObject pistolPrefab;
    public GameObject jetPackPrefab;

    public bool hasPistol;

    void Start()
    {
        pistolPrefab.SetActive(false);
        jetPackPrefab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pistolPrefab.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            jetPackPrefab.SetActive(true);
        }
    }
}
