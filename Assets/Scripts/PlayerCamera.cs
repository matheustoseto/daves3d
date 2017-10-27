using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : MonoBehaviour
{

    private void Start()
    {
        transform.localEulerAngles = new Vector3(30, 0, 0);
    }

}
