using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemRotator : NetworkBehaviour
{
	void Update () {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
	}
}
