using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    //Singleton ugly but fast
    public static GameCharacterController Instance
    {
        get => m_instance;
        set => m_instance = value;
    }

    [SerializeField] private float m_horizontalSpeed;
    [SerializeField] private float m_verticalSpeed;
    [SerializeField] private ToolConfig m_toolConfig;
    [SerializeField] GameObject m_toolRoot;
    ToolType m_equippedTool = ToolType.None;

    private ProceduralPieceSpawner _proceduralPieceSpawner;

    private ToolType EquippedTool
    {
        get => m_equippedTool;
    }

    Rigidbody m_rigidBody;
    Animator m_animator;
    static GameCharacterController m_instance;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();

        //there can only be one anyway
        m_instance = this;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        m_animator.SetBool("Run", false);

        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //no action?
        if (direction.magnitude == 0)
        {
            return;
        }

        Vector3 movement = new Vector3(0, direction.y * m_verticalSpeed * Time.deltaTime, direction.x * m_horizontalSpeed * Time.deltaTime);
        bool isMovementAllowed = !Physics.Raycast(transform.position, direction, movement.sqrMagnitude);
        if (isMovementAllowed)
        {
            transform.Translate(movement);
        }
        m_animator.SetBool("Run", isMovementAllowed);
    }

    public void SetEquipedTool(ToolType toolType)
    {
        m_equippedTool = toolType;
        for (int i = m_toolRoot.transform.childCount - 1; i >= 0; i--)
        {
            if (m_toolRoot.transform.GetChild(i).tag == "Tool")
                Destroy(m_toolRoot.transform.GetChild(i).gameObject);
        }

        var toolPrefab = m_toolConfig.GetToolAsset(toolType);
        if (toolPrefab != null)
        {
            var newToolObject = Instantiate<GameObject>(toolPrefab);
            newToolObject.transform.parent = m_toolRoot.transform;
            newToolObject.transform.localScale = Vector3.one * 0.25f;
            newToolObject.transform.localPosition = Vector3.zero;
            newToolObject.transform.localRotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (EquippedTool == ToolType.None)
            return;

        if (collision.gameObject.CompareTag("Tooth"))
        {
            var tooth = collision.gameObject.GetComponent<Tooth>();
            if (tooth)
            {
                if (m_equippedTool == tooth.FixTool)
                {
                    tooth.SetMaterial(m_toolConfig.GetMaterialForTooth(ToolType.None));
                    SetEquipedTool(ToolType.None);
                    ProceduralPieceSpawner.scoreValue += 100;
                    Debug.Log("The player's score has increase by 100 points");
                }
            }
        }
    }
}
