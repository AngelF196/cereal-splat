using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int numPlayers = 1;
    [SerializeField] GameObject playerPrefab;
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
                Instantiate(playerPrefab, position: playerSpawns[i], quaternion.identity);
            }
        }
    }
}
