using UnityEngine;
using System.Collections;

public class ThirdPersonCameraController : MonoBehaviour {

	Vector3 direction;
	float distance;

	void Awake()
	{
		direction = new Vector3(1, 0, -2);
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
			if(!Input.GetKey(KeyCode.E)){
			transform.position = hit.point + hit.normal * .3f;
			}
			//Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, hit.point + hit.normal * .3f,0.25f);
		}
		else
		{
			if(!Input.GetKey(KeyCode.E)){
				//transform.localPosition = direction;
				Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(1,0,-2),0.25f);
			}
		}
	}
}
