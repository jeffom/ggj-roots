using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tooth : MonoBehaviour
{
    //The tool that should be used to fix this teeth
    [SerializeField] public ToolType FixTool;
    [SerializeField] public MeshRenderer m_toothRenderer;

    [SerializeField] public Material healedTooth;

    public bool IsHealed()
    {
	    return m_toothRenderer.sharedMaterial == healedTooth;
    }

    public void SetMaterial(Material mat)
    {
        //I know it duplicates it but in this case it should be fine
        m_toothRenderer.material = mat;
    }
}
