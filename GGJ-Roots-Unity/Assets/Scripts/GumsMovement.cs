using UnityEngine;
using UnityEngine.Serialization;

public class GumsMovement : MonoBehaviour
{
	[FormerlySerializedAs("object1")] public GameObject gumsUpper;
	[FormerlySerializedAs("object2")] public GameObject gumsBottom;
	private const float MovementSpeed = 0.05f;
	private const float StoppingDistance = 5f;

	private void Update()
	{
		var position = gumsUpper.transform.position;
		var position1 = gumsBottom.transform.position;
		Vector3 direction = (position1 - position).normalized;
		float distance = Vector3.Distance(position, position1);

		if (distance > StoppingDistance)
		{
			gumsUpper.transform.position += direction * (MovementSpeed * Time.deltaTime);
			gumsBottom.transform.position -= direction * (MovementSpeed * Time.deltaTime);
		}
	}
}