using UnityEngine;
using System.Collections;

public class DisablePlayerActions2 : Photon.MonoBehaviour
{
	/*
	private GameObject player;
	private GameObject graphics;
	private GameObject weaponholder;
	GameObject gunrendererthroughobjects;
	GameObject maincamera;
	GameObject weaponwalkanimation;
	GameObject weaponjumpanimation;
	GameObject scarh;
	GameObject arm;
	GameObject laser;
	GameObject lasercircle;
	private MonoBehaviour charactermotorscript;
	private MonoBehaviour fpsinputcontrollerscript; 

	void Awake () {
		player= GameObject.Find("Player");
		graphics = GameObject.Find("Graphics");
		weaponholder = GameObject.Find ("WeaponHolder");
		gunrendererthroughobjects = GameObject.Find ("GunRendererThroughObjects");
		maincamera = GameObject.Find ("Main Camera");
		weaponwalkanimation = GameObject.Find ("WeaponWalkAnimation");
		weaponjumpanimation = GameObject.Find ("WeaponJumpAnimation");
		scarh = GameObject.Find ("Scar-H");
		arm = GameObject.Find ("Arm");
		laser = GameObject.Find ("Laser");
		lasercircle = GameObject.Find ("LaserCircle");
		//find the java scripts
		charactermotorscript=player.GetComponent("CharacterMotor") as MonoBehaviour;
		fpsinputcontrollerscript = player.GetComponent ("FPSInputController") as MonoBehaviour;


		if (maincamera != null) {
			Debug.Log("asdf");
		}
		if(gunrendererthroughobjects != null){

			Debug.Log("ssss");
		}

		if (charactermotorscript != null) {
			Debug.Log("111");

		}

		if (fpsinputcontrollerscript != null) {
			Debug.Log("222");
		}

	}
	void Update()
	{
		if(!photonView.isMine)
		{
			this.gameObject.layer = 10;

			//playerobjects
			this.GetComponent<GameManager>().enabled = false;
			this.GetComponent<PauseScript>().enabled = false;
			this.GetComponent<EndGame>().enabled = false;

			//player
			player.GetComponent<CharacterController>().enabled = false;
			player.GetComponent<MouseLook>().enabled = false;
			charactermotorscript.enabled = false;
			fpsinputcontrollerscript.enabled = false;
			//(this.GetComponentInChildren("CharacterMotor") as MonoBehaviour).enabled = false;
			//this.GetComponentInChildren<FPsInputContorller>().enabled = false;


			//graphics.GetComponent<MeshRenderer>().enabled = false;

			//WeaponHolder mouselock again? 
			weaponholder.GetComponent<MouseLook>().enabled = false;

			gunrendererthroughobjects.GetComponent<GUILayer>().enabled = false;
			gunrendererthroughobjects.GetComponent<Camera>().enabled = false;
			//gunrendererthroughobjects.GetComponent<Flare> = false;

			maincamera.GetComponent<Camera>().enabled = false;
			maincamera.GetComponent<GUILayer>().enabled = false;
			maincamera.GetComponent<AudioListener>().enabled = false;
			maincamera.GetComponent<PlayerScript>().enabled = false;
			maincamera.GetComponent<AudioSource>().enabled = false;

			weaponwalkanimation.GetComponent<Animation>().enabled = false;

			weaponjumpanimation.GetComponent<Animation>().enabled = false;

			scarh.GetComponent<MeshRenderer>().enabled = false;
			scarh.GetComponent<Animator>().enabled = false;

			arm.GetComponent<Animator>().enabled = false;
			arm.GetComponent<MeshRenderer>().enabled = false;

			laser.GetComponent<LaserScript>().enabled = false;

			lasercircle.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	*/
	
	
	void Update()
	{
		
		if(!photonView.isMine)
		{	
			this.gameObject.layer = 10;
			
			//playerobjects
			this.GetComponent<GameManager>().enabled = false;
			this.GetComponent<PauseScript>().enabled = false;
			this.GetComponent<EndGame>().enabled = false;

			/*
			//player
			this.GetComponentInChildren<CharacterController>().enabled = false;
			
			//this.GetComponentInChildren<MouseLook>().enabled = false;
			
			MouseLook [] ml =  this.GetComponentsInChildren<MouseLook>();
			foreach(MouseLook m in ml){
				m.enabled = false;
			}
			
			this.GetComponentInChildren<FPSInputController>().enabled = false;
			this.GetComponentInChildren<CharacterMotor>().enabled = false;
			*/
			this.GetComponent<CharacterController>().enabled = false;
			this.GetComponent<MouseLook>().enabled = false;
			this.GetComponent<CharacterMotor>().enabled = false;
			this.GetComponent<FPSInputController>().enabled = false;
			MouseLook [] ml =  this.GetComponentsInChildren<MouseLook>();
			foreach(MouseLook m in ml){
				m.enabled = false;
			}
			
			//charactermotorscript.enabled = false;
			//fpsinputcontrollerscript.enabled = false;
			//(this.GetComponentInChildren("CharacterMotor") as MonoBehaviour).enabled = false;
			//this.GetComponentInChildren<FPsInputContorller>().enabled = false;
			
			
			//graphics.GetComponent<MeshRenderer>().enabled = false;
			
			//WeaponHolder mouselock again? 
			//this.GetComponentInChildren<MouseLook>().enabled = false;
			
			
			Camera [] ca = this.GetComponentsInChildren<Camera>();
			foreach(Camera c in ca){
				c.enabled = false;
			}
			
			GUILayer [] gl = this.GetComponentsInChildren<GUILayer>();
			foreach(GUILayer g in gl){
				g.enabled = false;
			}
			
			AudioListener [] al = this.GetComponentsInChildren<AudioListener>();
			foreach(AudioListener a in al){
				a.enabled = false;
			}
			
			Animator [] at= this.GetComponentsInChildren<Animator>();
			foreach(Animator ani in at){
				ani.enabled = false;
			}
			
			Animation [] animation = this.GetComponentsInChildren<Animation>();
			foreach(Animation anim in animation){
				anim.enabled = false;
			}
			
			AudioSource [] asource = this.GetComponentsInChildren<AudioSource>();
			foreach(AudioSource Asource in asource){
				Asource.enabled = false;
			}
			
			//gunrendererthroughobjects
			//this.GetComponentInChildren<GUILayer>().enabled = false;
			//this.GetComponentInChildren<Camera>().enabled = false;
			//gunrendererthroughobjects.GetComponent<Flare> = false;
			
			//maincamera
			//this.GetComponentInChildren<Camera>().enabled = false;
			//this.GetComponentInChildren<GUILayer>().enabled = false;
			//this.GetComponentInChildren<AudioListener>().enabled = false;
			this.GetComponentInChildren<PlayerScript>().enabled = false;
			//this.GetComponentInChildren<AudioSource>().enabled = false;
			
			
			/*
			weaponwalkanimation.GetComponent<Animation>().enabled = false;
			
			weaponjumpanimation.GetComponent<Animation>().enabled = false;
			
			scarh.GetComponent<MeshRenderer>().enabled = false;
			scarh.GetComponent<Animator>().enabled = false;
			
			arm.GetComponent<Animator>().enabled = false;
			arm.GetComponent<MeshRenderer>().enabled = false;
			*/
			
			
			this.GetComponentInChildren<LaserScript>().enabled = false;
			
			
			//lasercircle.GetComponent<MeshRenderer>().enabled = false;
			
			
		}
	}
	
	
}
