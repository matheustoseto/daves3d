using UnityEngine;
using System.Collections;

public class ButtonSwitch : MonoBehaviour {

    public Animator animator;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            animator.SetBool("Press", true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            animator.SetBool("Press", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            animator.SetBool("Press", false);
        }
    }
}
