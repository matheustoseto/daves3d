using UnityEngine;
using System.Collections;

public class ShowItens : MonoBehaviour {

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
