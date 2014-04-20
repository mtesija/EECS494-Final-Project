using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour 
{
	public GameObject player;

	private float hitLength = 1;
	private Vector3 direction;

	void Awake()
	{
		direction = new Vector3(1, .9f, -2);
	}

	void Update() 
	{
		return;

		RaycastHit hit;
		if(Physics.Raycast(player.transform.position, direction, out hit, hitLength))
		{
			this.transform.position = hit.point + hit.normal * .5f;
		}
		else
		{
			this.transform.position = player.transform.position + direction;
		}
	}
}
