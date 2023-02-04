using UnityEngine;
using UnityEngine.Serialization;

public class EndGameCollisionDetector : MonoBehaviour
{
	[FormerlySerializedAs("otherObject")] public GameObject gumBottom;

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject == gumBottom)
		{
			Debug.Log("The two gums have collided!");
			MainMenu.TerminateGame();
		}
	}
}