using UnityEngine;
using UnityEngine.Serialization;

public class GumsMovement : MonoBehaviour
{
	[FormerlySerializedAs("object1")] public GameObject gumsUpper;
	[FormerlySerializedAs("object2")] public GameObject gumsBottom;
	private const float MovementSpeed = 2f;
	private static float stoppingDistance = 6.5f;
	private const float Step = 0.25f;

	private void Update()
	{
		var position = gumsUpper.transform.position;
		var position1 = gumsBottom.transform.position;
		Vector3 direction = (position1 - position).normalized;
		float distance = Vector3.Distance(position, position1);

		if (distance > stoppingDistance)
		{
			gumsUpper.transform.position += direction * (MovementSpeed * Time.deltaTime);
		}
	}

	public static void FailedStep()
	{
		stoppingDistance -= Step;
	}
}