using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public ToolType m_toolType;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameCharacterController.Instance.SetEquipedTool(m_toolType);
        }
    }
}
