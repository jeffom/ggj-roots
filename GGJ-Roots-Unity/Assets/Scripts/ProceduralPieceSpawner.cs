using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ProceduralPieceSpawner : MonoBehaviour
{
    public static ProceduralPieceSpawner Instance;
    
    [SerializeField] private GameObject m_mouthObject;
    [SerializeField] private GameObject m_container;
    [SerializeField] private Camera m_gameCamera;
    [SerializeField] private ToolConfig m_toolConfig;
    [SerializeField] private Transform m_scoreParent;
    [FormerlySerializedAs("score")] [SerializeField] public TextMeshProUGUI scoreText;

    public int scoreValue = 0;
    
    //How many units for 1 second
    [SerializeField] private float m_piecesSpeed;

    private int m_initialSize = 5;
    private float m_initialPieceLength = 10.5f;

    private List<GameObject> m_spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_initialSize; i++)
        {
            var instance = SpawnSection(m_initialPieceLength * i);
            m_spawnedObjects.Add(instance);
            scoreText.text = "Your score is: " + scoreValue;
        }
    }

    private void Update()
    {
	    scoreText.text = "Your score is: " + scoreValue;
    }

    void LateUpdate()
    {
        for (int i = 0; i < m_spawnedObjects.Count; i++)
        {
            m_spawnedObjects[i].transform.Translate(new Vector3(m_piecesSpeed * Time.deltaTime, 0, 0));
        }

        CheckDespawn();
        CheckSpawn();
    }

    void CheckSpawn()
    {
        var lastPiece = m_spawnedObjects[m_spawnedObjects.Count - 1];
        if (lastPiece.transform.position.x < 45)
        {
            var instance = SpawnSection(lastPiece.transform.position.x + m_initialPieceLength);

            m_spawnedObjects.Add(instance);
        }
    }

    void CheckDespawn()
    {
        var firstPiece = m_spawnedObjects[0];
        if (firstPiece.transform.position.x <= -20)
        {
            m_spawnedObjects.Remove(firstPiece);
            Destroy(firstPiece);
        }
    }
    
    GameObject SpawnSection(float startPosX)
    {
        var instance = GameObject.Instantiate(m_mouthObject);
        instance.transform.position += new Vector3(startPosX, 0, 0);
        instance.transform.parent = m_container.transform;

        var teeth = instance.GetComponentsInChildren<Tooth>();
        var enums = Enum.GetValues(typeof(ToolType));
        var random = new System.Random();

        foreach (var tooth in teeth)
        {
            ToolType toolType = (ToolType)enums.GetValue(random.Next(0, enums.Length));
            tooth.FixTool = toolType;
            Material mat = m_toolConfig.GetMaterialForTooth(toolType);
            GameObject diseaseObject = m_toolConfig.GetDiseaseObject(toolType);
            if (mat != null)
                tooth.SetMaterial(mat, diseaseObject);
        }

        return instance;
    }

    public void ShowScoreBlimp(int combo, int scoreGained)
    {
        GameObject tempBlimp = Instantiate(scoreText.gameObject, m_scoreParent);
        tempBlimp.transform.position += Vector3.down * 1;
        tempBlimp.GetComponent<TextMeshProUGUI>().text = combo + "x combo: " + scoreGained;
        tempBlimp.AddComponent<BlimpScript>();
    }
}
