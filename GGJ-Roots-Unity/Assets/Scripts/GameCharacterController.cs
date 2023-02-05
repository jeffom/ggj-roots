using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public string parameterName = "Anima_Param_Use_Tool";

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
        m_animator = GetComponentInChildren<Animator>();

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
            m_toolConfig.PlayRandomToolSound(toolType, newToolObject);
        }
    }
    
    public void TriggerAnimation(string triggerName)
    {
	    m_animator.SetTrigger(triggerName);
    }

    private void OnCollisionEnter(Collision collision)
    {
	    if (EquippedTool == ToolType.None)
            return;

	    if (collision.gameObject.CompareTag("Tooth"))
	    {
		    var tooth = collision.gameObject.GetComponent<Tooth>();
		    var currentCollisionMaterial = collision.gameObject.GetComponent<Tooth>().m_toothRenderer.material;
		    var previousCollisionMaterial = GameObject.Find("Tooth (1)").GetComponent<Tooth>().m_toothRenderer.material;
		    if (tooth)
		    {
			    if (m_equippedTool == tooth.FixTool)
			    {
				    tooth.SetMaterial(m_toolConfig.GetMaterialForTooth(ToolType.None));
				    tooth.PlayFixingSound();
				    TriggerAnimation("Anim_Param_Use_Tool");
				    ProceduralPieceSpawner.scoreValue += 100;
			    }

			    if (m_equippedTool == tooth.FixTool && previousCollisionMaterial == currentCollisionMaterial)
			    {
				    tooth.SetMaterial(m_toolConfig.GetMaterialForTooth(ToolType.None));
				    tooth.PlayFixingSound();
				    ProceduralPieceSpawner.scoreValue += 200;
				    Debug.Log("The player has received twice the points.");
			    }
		    }
	    }
    }
}
