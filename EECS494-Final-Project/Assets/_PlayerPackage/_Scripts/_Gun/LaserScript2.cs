using UnityEngine;
using System.Collections;

public class LaserScript2 : MonoBehaviour 
{
	private Color c1 = Color.red;
    private Color c2 = Color.red;

	public LineRenderer lineRenderer;
	public Texture2D laserTexture;
	private float rayDistance = 600;
	
	public GameObject laserCircle;
	public Ray ray; 

	void Awake()
	{
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.material.mainTexture = laserTexture;
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetWidth(0.03f, 0.03f);
	}
	
	void Update () 
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPosition(1, new Vector3(1, 0, 5000));
		
		ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		
		Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.blue);
		
		if(Physics.Raycast(ray, out hit, rayDistance))
		{			
			if(hit.collider)
			{
				Debug.Log(hit.distance);
				lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance) * 5); 	//Create a lineRenderer forward until the point it collides with a collidable object.
				laserCircle.renderer.enabled = true;							//Enable the lasercircle if it hits a collider.
				laserCircle.transform.position = hit.point + hit.normal * .05f;						//Put the lasercircle at the end of the hit.point of the raycast.
				laserCircle.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);	//Make sure the lasercircle is always "facing" an outwards position of where it impacts.
			}
		} 
		else 
		{																		//If raycast doesnt hit anything, make the linerenderer 5000 units long and disable the lasercircle.
			lineRenderer.SetPosition(1,new Vector3(0,0,5000));
			laserCircle.renderer.enabled = false;
		}
	}
}