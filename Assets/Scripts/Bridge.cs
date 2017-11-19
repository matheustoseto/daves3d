using UnityEngine;
using System.Collections;

public class Bridge : MonoBehaviour {

    public Animator animator;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("Firstmove");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("Secmove");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("On");
        }

    }
}
