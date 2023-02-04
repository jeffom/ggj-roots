using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothDetector : MonoBehaviour
{
	[SerializeField] private List<AudioClip> screams;

	public void OnTriggerEnter(Collider collision)
	{
		if (!collision.gameObject.TryGetComponent<Tooth>(out var tooth))
		{
			return;
		}

		if (!tooth.IsHealed())
		{
			GumsMovement.FailedStep();
			SoundPlayer.Instance.PlayRandomSoundOnGO(screams, Camera.main.gameObject, SoundPlayer.SFX);
		}
	}
}
