using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ToolType
{
    Tool1,
    Tool2,
    Tool3,
    Tool4,
    None
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ToolConfig", order = 1)]
public class ToolConfig : ScriptableObject
{
    [System.Serializable]
    public class ToolAsset
    {
        [SerializeField] public ToolType Type;
        [SerializeField] public GameObject Prefab;
        [SerializeField] public Material ToothMaterial;
        [SerializeField] public List<AudioClip> ToolSounds;
    }

    [SerializeField]
    private ToolAsset[] m_toolMap;

    public GameObject GetToolAsset(ToolType type)
    {
        var toolPrefab = m_toolMap.FirstOrDefault(x => x.Type == type);
        return toolPrefab != null ? toolPrefab.Prefab : null;
    }

    public Material GetMaterialForTooth(ToolType type)
    {
        var toolPrefab = m_toolMap.FirstOrDefault(x => x.Type == type);
        return toolPrefab != null ? toolPrefab.ToothMaterial : null;
    }

    public void PlayRandomToolSound(ToolType type, GameObject GO)
    {
        var toolPrefab = m_toolMap.FirstOrDefault(x => x.Type == type);
        List<AudioClip> toolSounds = toolPrefab != null ? toolPrefab.ToolSounds : null;

        SoundPlayer.Instance.PlayRandomSoundOnGO(toolSounds, GO, SoundPlayer.SFX);
    }
}
