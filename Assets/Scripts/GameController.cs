using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private GameObject player;
    public GameObject door;
    public Material materialOpenDoor;

    public int Lifes = 3;
    public bool removeLife = false;
    public Texture2D davesLife;
    public Texture2D openDoorTexture;
    public Texture2D gunTexture;
    public Texture2D jackpackTexture;
    public bool openDoor = false;

    private string fmt = "00000";
    public int score = 0;
    public Font font;

    public static int currentStage;

    // Use this for initialization
    void Start () {
        currentStage = 1;
    }

    // Update is called once per frame
    void Update () {
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        door = GameObject.FindGameObjectWithTag("Door");
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name.Equals("Menu"))
            Destroy(gameObject);

        door = GameObject.FindGameObjectWithTag("Door");
        player = GameObject.FindGameObjectWithTag("Player");
        openDoor = false;
        currentStage++;
    }

    public void OpenDoor() {
        door.GetComponent<MeshRenderer>().material = materialOpenDoor;
        door.GetComponent<LoadScene>().getCup = true;
        openDoor = true;
    }

    void OnGUI()
    {
        int space = 20;
        GUI.skin.font = font;
        GUI.skin.label.fontSize = 40;
     
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        if(Lifes > 0)
        {
            GUI.Label(new Rect(15, 15, 300, 50), score.ToString(fmt));
            for (int i = 0; i < Lifes; i++)
            {
                GUI.Label(new Rect(Screen.width - 200 + (30 * i) + space, 15, 64, 32), davesLife);
                space += 20;
            }
            GUI.Label(new Rect((Screen.width / 2) - (150 / 2), 15, 300, 50), "Level : " + currentStage);

            if(player.GetComponent<Player>().getPistol)
                GUI.Label(new Rect(15, Screen.height - 50, 300, 50), gunTexture);

            if (openDoor)
                GUI.Label(new Rect(Screen.width - 271, Screen.height - 45, 400, 100), openDoorTexture);

            if(player.GetComponent<Player>().getJackPack)
                GUI.Label(new Rect((Screen.width / 2) - (150 / 2), Screen.height - 45, 300, 50), jackpackTexture);
        }
        else
        {
            GUI.Label(new Rect((Screen.width / 2) - (200 / 2), Screen.height / 2 - 100, 300, 100), "GameOver");

            if (GUI.Button(new Rect((Screen.width / 2) - (500 / 2), Screen.height / 2, 500, 100), "Clique aqui para voltar ao menu!"))
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        GUI.EndGroup();
    }

    public void AddScore(int scr)
    {
        score += scr;
    }

    public void RemoveLife()
    {
        if (!removeLife)
        {
            removeLife = true;
            Lifes--;
            player.transform.position = player.GetComponent<Player>().startPoint;

            if (Lifes <= 0)
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            removeLife = false;
        }     
    }
}
