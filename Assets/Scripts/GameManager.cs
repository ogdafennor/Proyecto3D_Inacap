using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] prefabsToSpawn;
    private int totalSpawnear = 15;
    private Vector3 spawnArea = new Vector3(50, 0, 50);
    private float spawnY = 1f;

    public int score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        spawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnObjects()
    {
        for (int i = 0; i < totalSpawnear; i++)
        {
            GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
            Vector3 pos = new Vector3(Random.Range(-spawnArea.x, spawnArea.x), spawnY, Random.Range(-spawnArea.z, spawnArea.z));

            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    public void addScore(int puntaje)
    {
        score += puntaje;
    }
    public void resetScore()
    {
        score = 0;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resetScore();
        spawnObjects();
    }
}
