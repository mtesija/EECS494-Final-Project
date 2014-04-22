using UnityEngine;
using System.Collections;

public class ThirdPersonCameraController2 : MonoBehaviour {

	Vector3 direction;
	float distance;

	void Awake()
	{
		direction = new Vector3(1, .9f, -2);
		distance = Mathf.Sqrt(5);
	}

	void Update()
	{
		Vector3 desiredDirection = transform.parent.TransformDirection(direction);

		int playerLayer = 1 << LayerMask.NameToLayer("Player");
		int otherPlayerLayer = 1 << LayerMask.NameToLayer("OtherPlayers");
		int shieldLayer = 1 << LayerMask.NameToLayer("Shield");
		int layerMask = playerLayer | otherPlayerLayer | shieldLayer;
		layerMask = ~layerMask;

		RaycastHit hit;
		if(Physics.Raycast(transform.parent.position, desiredDirection, out hit, distance, layerMask))
		{
			transform.position = hit.point + hit.normal * .3f;
		}
		else
		{
			transform.localPosition = direction;
		}
	}
}
