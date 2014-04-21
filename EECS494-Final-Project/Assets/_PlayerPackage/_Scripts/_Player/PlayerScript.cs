using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour 
{

	public PauseScript pauseScript;
	float bulletSpeed = 10;
	public GameObject bullet;
	public CharacterController CharCont;
	public CharacterMotor CharMotor;
	public Transform WeaponHolder;
	public Transform FPSCharacter;
	public Transform WalkAnimationHolder;
	public Transform JumpAnimationHolder;
	public Transform ADSHolder;
	public Transform RecoilHolder;
	public Transform RecoilHolderCamera;
	public Transform leftarmref;
	public Transform rightarmref;
	
	//Player Variables
	public WalkingState walkingState = WalkingState.Idle;
	public float velocityMagnitude;
	public float walkSpeed;
	public float runSpeed;
	public bool WasStanding;
	public bool isAiming;
	
	//Creating the Red Laser
	private float rayDistance = 600;
	public float distanceToObject;
	
	//Shooting with your Weapon.
	public List<WeaponInfo> WeaponList = new List<WeaponInfo>();
	public WeaponInfo CurrentWeapon;
	public int bulletCounter = 30;			//Number of bullets.
	public int magazineCounter = 99;		//Number of magazines.
	public float shootTimer = 0f;
	public float refireTimer = 1f;
	private float reloadTimer = 1.75f;
	private float reloadDone = 0;
	private bool reloadWeapon = false;
	
	public Vector3 CurrentRecoil1;		//x-Rotation
	public Vector3 CurrentRecoil2;		//y-Rotation
	public Vector3 CurrentRecoil3;		//x-Kickback
	public Vector3 CurrentRecoil4;		//y-Kickback
	
	public AudioClip gunFire;
	public AudioClip emptyGun;
	
	public GameObject[] impactArray = new GameObject[5];
	public GameObject[] muzzleArray = new GameObject[10];
	
	//Timer is used to stop the focus mechanic of the players gun for the first 0.3 sec, so the gun wont act weird.
	private float AdjustTimer = 0.3f;
	private float baseAdjustTimer = 0.3f;

	private PlayerDataScript playerData;

	private LaserScript laserScript;
	private GameObject barrel;
	public Animator anim;
	void Start()
	{	
		pauseScript = FindObjectOfType(typeof(PauseScript)) as PauseScript;

		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ();
		
		CurrentWeapon = WeaponList[0];
		//laserScript = gameObject.GetComponentInChildren<LaserScript> ();
		laserScript = gameObject.transform.root.GetComponentInChildren<LaserScript> ();
		barrel = GameObject.Find("Barrel");
		anim = gameObject.transform.root.GetComponentInChildren<Animator> ();
	}
	
	public void FixedUpdate()
	{
		//Debug.Log(barrel.transform.position);
		if(!pauseScript.pause)
		{
			AdjustTimer -= 1 * Time.deltaTime;
			if(AdjustTimer <= 0f)
			{
				GunController();	
				AdjustTimer = 0f;
			}
			
			ADSController();
			//RecoilController();
			velocityMagnitude = CharCont.velocity.magnitude;
		}
		
		SpeedController();
		AnimationController();
	}
	
	public void GunController()
	{
		shootTimer += Time.deltaTime * 1; //Start at 0, reset after bullet has fired.
		
		//Raycast from middle of camera to middle of screen.
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
		RaycastHit hit;
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red);
		
		if(Physics.Raycast(ray, out hit, rayDistance) && walkingState != WalkingState.Running && !Input.GetButton("Fire2"))
		{				
			if(!isAiming)
			{
				if(hit.distance >= 4) // Cap its minimum distance to 4, so that your gun wont glitch out of position.
				{
					Vector3 relativePos = hit.point - CurrentWeapon.WeaponTransform.transform.position;
					Quaternion rotation = Quaternion.LookRotation(relativePos);
					//CurrentWeapon.WeaponTransform.transform.rotation = rotation;
					
					//if the distance is smaller than 4, rotate the Gun towards the middle of the screen.
					CurrentWeapon.WeaponTransform.transform.rotation = Quaternion.Slerp(CurrentWeapon.WeaponTransform.transform.rotation, rotation, Time.deltaTime * 20f);

				}
			}
			else 
			{

				if(hit.distance >= 4) // Cap its minimum distance to 4, so that your gun wont glitch out of position.
				{
					//Uncomment these lines if you want your ADS to focus on targets and keeping the laser on your objects. This will cause your Sight to jump out of your FOV.
					Vector3 relativePos = hit.point - CurrentWeapon.WeaponTransform.transform.position;
					Quaternion rotation = Quaternion.LookRotation(relativePos);
					CurrentWeapon.WeaponTransform.transform.rotation = Quaternion.Slerp(CurrentWeapon.WeaponTransform.transform.rotation, rotation, Time.deltaTime * 7f);
				} 
				
				//Comment this line if you want your ADS to go back to its forward position at all times. It wont focus on new targets. Uncomments the above if-statement.
				//CurrentWeapon.WeaponTransform.transform.rotation = transform.rotation;
			}
			
			if(Input.GetButton("Fire1") && reloadWeapon == false && walkingState != WalkingState.Running)
			{
				if(shootTimer >= refireTimer)
				{
					if(bulletCounter > 0)
					{	
						anim.SetTrigger("fire");
						GameObject bullet = PhotonNetwork.Instantiate("Bullet", barrel.transform.position, this.transform.rotation, 0) as GameObject;
						bullet.rigidbody.velocity = 10*laserScript.ray.direction;

						PhotonView bulletView = bullet.GetComponent<PhotonView>();
						bulletView.RPC("SetColor", PhotonTargets.All, playerData.playerColor.r, playerData.playerColor.g, playerData.playerColor.b, playerData.playerColor.a);

						/*
						GameObject redBullet = Instantiate(Resources.Load("Bullet"), this.transform.position, Quaternion.identity) as GameObject;
						redBullet.rigidbody.velocity = ray.direction;
						BulletScript redBulletScript = redBullet.GetComponent<BulletScript>();
						*/

						//Spawn Muzzles at the end of the gun barrel.

						/*
						GameObject _Muzzle;
						_Muzzle = Instantiate(muzzleArray[Random.Range(0,10)], CurrentWeapon.gunBarrelPoint.transform.position, CurrentWeapon.gunBarrelPoint.transform.rotation) as GameObject; //Make a MuzzleFlash. //Rotate it toward the gun.
						_Muzzle.transform.parent = WeaponHolder.transform;
						muzzleArray[2].layer = 8; //Layer see gunfire through objects. (Layer 8 = GunThroughObjects layer).
						*/

						//If the target collider doesnt have the CollidableScript component attached, spawn an impact prefab on the hit.normal.
						/*
						if(!hit.collider.gameObject.GetComponent<CollidableScript>())
						{
							GameObject _Impact;
							_Impact = Instantiate(impactArray[Random.Range(0,5)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; //This will make an impact at the point where the raycast hits, and it will turn up from the surface on which is being shot at.									
							_Impact.transform.Translate(0.0f, 0.01f, 0.0f);
							//Position the object 0.01f upwards, so that the plane prefab wont get "stuck" into other surfaces.
						}
						*/
						/*
						CollidableScript collidable = hit.collider.gameObject.GetComponent<CollidableScript>();
						if (collidable)
						{
							//If the enemy collider has a collidable script attached, check if its an enemy or a hostage.
							if(hit.collider.gameObject.GetComponent<EnemyScript>())
							{
								DataManager.getInstance().enemiesKilled += 1;
								hit.collider.gameObject.GetComponent<EnemyScript>().isShot = true;
								Destroy(hit.collider.gameObject.GetComponent<CollidableScript>());
								//Destroy the component on that object, so that it wont be triggered twice when shot at.
							}
							
							else if(hit.collider.gameObject.GetComponent<HostageScript>())
							{
								hit.collider.gameObject.GetComponent<HostageScript>().isShot = true;
								DataManager.getInstance().friendliesKilled += 1;
								Destroy(hit.collider.gameObject.GetComponent<CollidableScript>());
								//Destroy the component on that object, so that it wont be triggered twice when shot at.
							}
						}	
						*/

						//Recoil while shooting.
						/*
						if(!isAiming)
						{
							CurrentRecoil1 += new Vector3(CurrentWeapon.RecoilRotation.x, Random.Range(-CurrentWeapon.RecoilRotation.y, CurrentWeapon.RecoilRotation.y));
							CurrentRecoil3 += new Vector3(Random.Range(-CurrentWeapon.RecoilKickBack.x, CurrentWeapon.RecoilKickBack.x), Random.Range(-CurrentWeapon.RecoilKickBack.y, CurrentWeapon.RecoilKickBack.y), CurrentWeapon.RecoilKickBack.z);
						}
						else 
						{
							CurrentRecoil1 += new Vector3(CurrentWeapon.RecoilRotation.x, Random.Range(-CurrentWeapon.RecoilRotation.y, CurrentWeapon.RecoilRotation.y));
							CurrentRecoil3 += new Vector3(Random.Range(-CurrentWeapon.RecoilKickBackADS.x, CurrentWeapon.RecoilKickBackADS.x), Random.Range(-CurrentWeapon.RecoilKickBackADS.y, CurrentWeapon.RecoilKickBackADS.y), CurrentWeapon.RecoilKickBackADS.z);
						}
						*/
						audio.PlayOneShot(gunFire);
						bulletCounter -= 1;
						shootTimer = 0;
						//Set the shoottimer back to 0, waiting for it to reach its cap again, so that the player can shoot once more.
					}
					
					if(bulletCounter == 0)
					{
						audio.PlayOneShot(emptyGun);
						shootTimer = 0;
					}
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.R) && walkingState != WalkingState.Running)
		{
			//If not running, reload the gun.
			if(magazineCounter > 0 && reloadWeapon == false)
			{
				reloadWeapon = true;
				magazineCounter -= 1;
			}
		}
		
		if(reloadWeapon)
		{
			reloadTimer -= Time.deltaTime;
			if(reloadTimer < reloadDone)
			{
				reloadWeapon = false;
				reloadTimer = 1.75f;
				bulletCounter = 31;
			}
		}
	}
	
	public void SpeedController()
	{
		//Adjusting the speed of the character controller.
		if((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && velocityMagnitude > 0)
		{
			if(Input.GetButton("Run") && !isAiming)
			{
				anim.SetBool("run",true);
				/*
				Quaternion target = Quaternion.Euler(0, -90f, 0);

				ADSHolder.transform.rotation = target;
				walkingState = WalkingState.Running;
				*/

				CharMotor.movement.maxForwardSpeed = runSpeed;
				CharMotor.movement.maxSidewaysSpeed = runSpeed;
				CharMotor.movement.maxBackwardsSpeed = runSpeed / 2;
				
				AdjustTimer = baseAdjustTimer;
			}
			else 
			{
				anim.SetBool("run",false);
				if(!isAiming)
				{
					walkingState = WalkingState.Walking;
				
					CharMotor.movement.maxForwardSpeed = walkSpeed;
					CharMotor.movement.maxSidewaysSpeed = walkSpeed;
					CharMotor.movement.maxBackwardsSpeed = walkSpeed / 2;
				}
				
				else if(isAiming)
				{
					walkingState = WalkingState.Walking;
				
					CharMotor.movement.maxForwardSpeed = walkSpeed / 10;
					CharMotor.movement.maxSidewaysSpeed = walkSpeed / 10;
					CharMotor.movement.maxBackwardsSpeed = walkSpeed / 10;
				}
			}
			if(Input.GetButton("Fire2")){
				CharMotor.movement.maxForwardSpeed = 0;
				CharMotor.movement.maxSidewaysSpeed = 0;
				CharMotor.movement.maxBackwardsSpeed = 0;
			}
		}
		else 
		{
			walkingState = WalkingState.Idle;
		}	
	}
	
	public void ADSController()
	{

		//Aim Down Sight controller. When right mouse clicked, lerp the gun towards the player his face.
		if(Input.GetKey(KeyCode.E) && walkingState == WalkingState.Walking || Input.GetKey(KeyCode.E) && walkingState == WalkingState.Idle)
		{
			isAiming = true;
			//ADSHolder.localPosition = Vector3.Lerp(ADSHolder.localPosition, CurrentWeapon.Scopes[CurrentWeapon.CurrentScope].adsPosition, 0.25f);
			Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0,0,1),0.25f);

		}
		else
		{
			isAiming = false;
			Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(1,0,-2),0.25f);
			//ADSHolder.localPosition = Vector3.Lerp(ADSHolder.localPosition, Vector3.zero, 0.25f);
		}

			
	}
	
	public void AnimationController()
	{

		//Switch between multiple character animations when correct parameters have been met. Fade between animations with a 0.2f speed.
		/*
		if(reloadWeapon == true)
		{
			WalkAnimationHolder.animation.CrossFade("Reload", 0.2f);
		}
		
			if(WasStanding && !CharCont.isGrounded)
			{
				WasStanding = false;
				JumpAnimationHolder.animation.Play("JumpUnzoomed");
			}
			else if(!WasStanding && CharCont.isGrounded)
			{
				WasStanding = true;
				JumpAnimationHolder.animation.Play("LandUnzoomed");
			}
		
		if(reloadWeapon == false)
		{	
			if(walkingState == WalkingState.Running)
			{
				WalkAnimationHolder.animation.CrossFade("RunUnzoomed", 0.2f);
			}
			else if(walkingState == WalkingState.Walking && !isAiming)
			{
				WalkAnimationHolder.animation.CrossFade("WalkUnzoomed", 0.2f);
			}
			
			else if(walkingState == WalkingState.Walking && isAiming)
			{
				WalkAnimationHolder.animation.CrossFade("WalkZoomed", 0.2f);
			}
			
			else if(walkingState == WalkingState.Idle && !isAiming)
			{
				WalkAnimationHolder.animation.CrossFade("IdleUnzoomed", 0.2f);
			}
			
			else if(walkingState == WalkingState.Idle && isAiming)
			{
				WalkAnimationHolder.animation.CrossFade("IdleZoomed", 0.2f);
			}		
		}
		*/

		if (Input.GetAxis ("Horizontal") < 0) {
			anim.SetBool("lft",true);
			anim.SetBool("rgt",false);
		}
		else if (Input.GetAxis ("Horizontal") > 0) {
			anim.SetBool("rgt",true);
			anim.SetBool("lft",false);
		}
		else{
			anim.SetBool("rgt",false);
			anim.SetBool("lft",false);
		}
		if (Input.GetAxis ("Vertical") > 0) {
			anim.applyRootMotion = true;
			anim.SetBool("fwd",true);
			anim.SetBool("bwd",false);
			anim.applyRootMotion = false;
		}
		else if (Input.GetAxis ("Vertical") < 0) {
			anim.SetBool("fwd",false);
			anim.SetBool("bwd",true);
		}
		else{
			anim.SetBool("fwd",false);
			anim.SetBool("bwd",false);
		}
		if (CharCont.isGrounded == false && Input.GetButton("Jump")) {
			anim.SetBool("jump", true);
		}
		else {
			anim.SetBool("jump", false);		
		}

		if(Input.GetKeyDown(KeyCode.Q)){
			anim.applyRootMotion = true;
			anim.SetTrigger("roll");
			anim.applyRootMotion = false;
		}

		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			anim.SetBool ("crouch", true);
		} 

		if (Input.GetKeyUp (KeyCode.LeftControl)) {
			anim.SetBool ("crouch", false);	
		}

		if(Input.GetButton("Fire2")) {
			anim.SetBool ("shield", true);
			/*
			ADSHolder.transform.position = new Vector3(-0.0835f,0.313f,-0.00117f);
			leftarmref.transform.position = new Vector3(-0.7445076f,-0.0347f,-0.3105f);
			rightarmref.transform.position = new Vector3(0.75358f,0.3511465f,0.10297f);
			*/
		}
		else {
			anim.SetBool ("shield", false);
		}

		if(Input.GetKeyDown(KeyCode.T)){
		    anim.applyRootMotion = true;
			anim.SetBool ("hitfront", true);
			anim.applyRootMotion = false;
	     }

		//if (anim.GetBool ("ground") == true) {
		//	CharCont.
		//}

	}
	public void RecoilController()
	{
		//Different Recoil values when player is aiming or free-firing.
		if(!isAiming)
		{
			CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, 0.1f);	
			CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, 0.1f);
			CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, 0.1f);
			CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, 0.2f);
			
			RecoilHolder.localEulerAngles = CurrentRecoil2;
			RecoilHolder.localPosition = CurrentRecoil4;
			
			RecoilHolderCamera.localEulerAngles = CurrentRecoil2 / 1f;		//Camera Recoil Sensitivity.
			
			camera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 7f);
			//While not aiming, Lerp the field of view of the player towards a wider range. (To focus on the environment).
			
			WeaponHolder.GetComponent<MouseLook>().sensitivityX = 6f; 	//Base SensitivityX = 6;
			WeaponHolder.GetComponent<MouseLook>().sensitivityY = 6f;	//Base SensitivityY = 6;
			FPSCharacter.GetComponent<MouseLook>().sensitivityX = 10f;	//Base SensitivityX = 10;
		}
		
		else if(isAiming)
		{
			CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, 0f);	
			CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, 0f);
			CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, 0.35f);
			CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, 0.35f);
			
			RecoilHolder.localEulerAngles = CurrentRecoil2;
			RecoilHolder.localPosition = CurrentRecoil4;
			
			camera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 20, Time.deltaTime * 7f);
			//While aiming, Lerp the field of view of the player towards a smaller range. (To focus on objects).
			
			WeaponHolder.GetComponent<MouseLook>().sensitivityX = 3f;	//Base SensitivityX / 2 = 3 sensitivity;
			WeaponHolder.GetComponent<MouseLook>().sensitivityY = 3f;	//Base SensitivityY / 2 = 3 sensitivity;
			FPSCharacter.GetComponent<MouseLook>().sensitivityX = 5f;	//Base SensitivityX / 2 = 5 sensitivity;
		}
	}
}

[System.Serializable] 	//Add this to show the WeaponInfo vars in the inspector, in its own editor bracket.
public class WeaponInfo
{
	public string name = "Weapon";
	
	public Transform WeaponTransform;
	
	public Vector3 RecoilRotation;
	public Vector3 RecoilKickBack;
	public Vector3 RecoilKickBackADS;
	
	public Transform gunLaserPoint;
	public Transform gunBarrelPoint;
	
	public int CurrentScope;
	public List<WeaponScope> Scopes = new List<WeaponScope>();
}

[System.Serializable] 	//Add this to show the WeaponScope vars in the inspector, in its own editor bracket.
public class WeaponScope
{
	public string name;
	public float fieldofView;
	public Vector3 adsPosition;
	public Transform scopeTransform;
}

//Character States.
public enum WalkingState
{
	Idle,
	Walking,
	Running
}