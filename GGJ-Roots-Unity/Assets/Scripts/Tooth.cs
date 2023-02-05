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
    [SerializeField] private List<AudioClip> pointsGrantedSound;
    [SerializeField] private List<AudioClip> fixingSounds;

    [SerializeField] public Material healedTooth;

    private GameObject diseaseInstance;

    public bool IsHealed()
    {
	    return m_toothRenderer.sharedMaterial == healedTooth;
    }

    public void SetMaterial(Material mat, GameObject diseaseObject = null)
    {
        //I know it duplicates it but in this case it should be fine
        m_toothRenderer.material = mat;
        if (diseaseObject != null)
        {
            diseaseInstance = Instantiate(diseaseObject, transform);
            diseaseInstance.transform.position -= new Vector3(0,0,3);
            diseaseInstance.transform.localScale *= 1.5f;
            return;
        }
        if (diseaseInstance) Destroy(diseaseInstance);
    }

    public void PlayFixingSound()
    {
        SoundPlayer.Instance.PlayRandomSoundOnGO(pointsGrantedSound, Camera.main.gameObject, SoundPlayer.SFX);
        SoundPlayer.Instance.PlayRandomSoundOnGO(fixingSounds, gameObject, SoundPlayer.SFX);
    }
}
