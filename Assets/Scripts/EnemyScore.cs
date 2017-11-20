using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyScore : NetworkBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("MultiBullet"))
        {
            MultiGameController.Instance.AddScore(transform.root.GetComponent<EnemyMultiplayer>().score);
            PlayerNetworkSetup.Instance.CmdDestroy(transform.root.gameObject);
            PlayerNetworkSetup.Instance.CmdDestroy(other.gameObject);
        }
    }
}
