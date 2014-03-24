using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	private float speed = 5;

	public float x = 1;
	public float y = -1;

	void Start()
	{
		this.rigidbody.velocity = new Vector3(x, 0, y).normalized * speed;
	}

	void FixedUpdate()
	{
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, .1f))
		{
			this.rigidbody.velocity = Vector3.Reflect(this.rigidbody.velocity, hit.normal);
		}
	}
}
