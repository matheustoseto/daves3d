using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public GridLayoutGroup Grid;

    private void Start()
    {
        Grid = GameObject.FindGameObjectWithTag("Panel").GetComponent<GridLayoutGroup>();
        gameObject.transform.SetParent(Grid.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
