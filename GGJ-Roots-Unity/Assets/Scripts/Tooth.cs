using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    //The tool that should be used to fix this teeth
    [SerializeField] public ToolType FixTool;
    //The tool that should not be used to fix the tooth
    [SerializeField] private ToolType WrongTool; 
    [SerializeField] public MeshRenderer m_toothRenderer;

    public void SetMaterial(Material mat)
    {
        //I know it duplicates it but in this case it should be fine
        m_toothRenderer.material = mat;
    }
}
