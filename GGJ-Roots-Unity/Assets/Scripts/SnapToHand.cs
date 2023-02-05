using UnityEngine;

public class SnapToHand : MonoBehaviour
{
	public static Transform handTransform;

	private void Update()
	{
		transform.position = handTransform.position;
		transform.rotation = handTransform.rotation;
		transform.SetParent(handTransform);
	}
}