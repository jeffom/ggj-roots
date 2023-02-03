using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_mouthObject;
    [SerializeField] private GameObject m_container;
    [SerializeField] private Camera m_gameCamera;
    //How many units for 1 second
    [SerializeField] private float m_piecesSpeed;

    private int m_initialSize = 5;  
    private float m_initialPieceLength = 10.5f;

    private List<GameObject> m_spawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_initialSize; i++)
        {
            var instance = GameObject.Instantiate(m_mouthObject);
            instance.transform.position += new Vector3(m_initialPieceLength * i, 0, 0);
            instance.transform.parent = m_container.transform;
            m_spawnedObjects.Add(instance);
        }
    }

    void LateUpdate()
    {
        for (int i=0; i < m_spawnedObjects.Count; i++)
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
            var instance = GameObject.Instantiate(m_mouthObject);
            instance.transform.position += new Vector3(lastPiece.transform.position.x + m_initialPieceLength, 0, 0);
            instance.transform.parent = m_container.transform;
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
}
