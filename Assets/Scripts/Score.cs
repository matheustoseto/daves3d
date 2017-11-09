using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Score : MonoBehaviour
{
    public int points = 10;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
    }
}
