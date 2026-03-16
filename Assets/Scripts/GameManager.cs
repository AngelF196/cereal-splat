using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int numPlayers = 1;
    public int winningIndex = 0;
    [SerializeField] GameObject playerPrefab;
    public PlayerControls[] allPlayerControls;
    private List<Vector3> playerSpawns = new List<Vector3>();

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        playerSpawns.Add(new Vector3 (-8.53f, -1.775f, 0f));
        playerSpawns.Add(new Vector3 (8.53f, -1.775f, 0f));
        playerSpawns.Add(new Vector3 (0f, -5.235001f, 0f));
        playerSpawns.Add(new Vector3 (0f, 1.635f, 0f));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game Scene")
        {
            for (int i = 0; i < numPlayers; i++) 
            {
                GameObject player = Instantiate(playerPrefab, position: playerSpawns[i], quaternion.identity);
                player.GetComponent<PlayerStats>().Initialize(i);

            }
        }
        if (scene.name == "Results")
        {
            switch (winningIndex) 
            {
                case 0:
                    GameObject playerRed = Instantiate(playerPrefab, position: new Vector3(2.73f, 0, 0), quaternion.identity);
                    playerRed.GetComponent<PlayerStats>().Initialize(0);
                    break;
                case 1:
                    GameObject playerBlue = Instantiate(playerPrefab, position: new Vector3(2.73f, 0, 0), quaternion.identity);
                    playerBlue.GetComponent<PlayerStats>().Initialize(1);
                    break;
                case 2:
                    GameObject playerYellow = Instantiate(playerPrefab, position: new Vector3(2.73f, 0, 0), quaternion.identity);
                    playerYellow.GetComponent<PlayerStats>().Initialize(2);
                    break;
                case 3:
                    GameObject playerGreen = Instantiate(playerPrefab, position: new Vector3(2.73f, 0, 0), quaternion.identity);
                    playerGreen.GetComponent<PlayerStats>().Initialize(3);
                    break;
            }
        }
        numPlayers = 0;
    }
}
