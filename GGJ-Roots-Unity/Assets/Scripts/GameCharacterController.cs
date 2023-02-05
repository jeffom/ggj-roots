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
    [SerializeField] AudioSource m_jetpackAudioSource;
    [SerializeField] float m_jetPackSoundTresholdHeight;

    ToolType m_equippedTool = ToolType.None;

    private Material comboMaterial = null;
    private int comboCounter = 1;

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
        m_jetpackAudioSource = GetComponent<AudioSource>();

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

        Vector3 movement = new Vector3(0, direction.y * m_verticalSpeed * Time.deltaTime,
            direction.x * m_horizontalSpeed * Time.deltaTime);
        bool isMovementAllowed = !Physics.Raycast(transform.position, direction, movement.sqrMagnitude);
        if (isMovementAllowed)
        {
            transform.Translate(movement);
        }

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, 1 << 6 );
        if (hit.transform != null)
        {
            Vector3 dist = transform.position - hit.point;           
            float volume = dist.y / m_jetPackSoundTresholdHeight;
            m_jetpackAudioSource.volume = Mathf.Clamp01(volume);
        }
               
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
            Destroy(newToolObject.GetComponent<Collider>());
            newToolObject.transform.parent = m_toolRoot.transform;
            newToolObject.transform.localScale = Vector3.one * 0.25f * 0.01f;
            newToolObject.transform.localPosition = Vector3.zero;
            newToolObject.transform.localRotation = Quaternion.identity;
            m_toolConfig.PlayRandomToolSound(toolType, newToolObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (EquippedTool == ToolType.None) return;
        if (!collision.gameObject.CompareTag("Tooth")) return;
        var tooth = collision.gameObject.GetComponent<Tooth>();
        if (!tooth) return;
        if (m_equippedTool != tooth.FixTool) return;

        var currentCollisionMaterial = collision.gameObject.GetComponent<Tooth>().m_toothRenderer.sharedMaterial;
        if (currentCollisionMaterial == m_toolConfig.GetMaterialForTooth(ToolType.None)) return;

        UpdateCombo(currentCollisionMaterial);

        int scoreForTooth = 100 * comboCounter;

        ProceduralPieceSpawner.Instance.scoreValue += scoreForTooth;
        ProceduralPieceSpawner.Instance.ShowScoreBlimp(comboCounter, scoreForTooth);

        tooth.SetMaterial(m_toolConfig.GetMaterialForTooth(ToolType.None));
        tooth.PlayFixingSound();
    }

    private void UpdateCombo(Material currentCollisionMaterial)
    {
        //Debug.Log(currentCollisionMaterial.name);
        //Debug.Log(comboCounter);

        if (comboMaterial != null && currentCollisionMaterial.name == comboMaterial.name)
        {
            comboCounter += 1;
            return;
        }

        comboCounter = 1;
        comboMaterial = currentCollisionMaterial;
    }
}