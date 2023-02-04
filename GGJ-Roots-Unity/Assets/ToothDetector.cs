using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothDetector : MonoBehaviour
{
	public void OnTriggerEnter(Collider collision)
	{
		if (!collision.gameObject.TryGetComponent<Tooth>(out var tooth))
		{
			return;
		}

		if (!tooth.IsHealed())
		{
			GumsMovement.FailedStep();
		}
	}
}
