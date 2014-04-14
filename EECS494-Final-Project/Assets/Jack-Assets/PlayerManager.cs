using UnityEngine;
using System.Collections;

/* 	PLAYER MANAGER
	========================================================================================================================
	DESCRIPTION:
	Over the past week or so, I have been working on a tool that helps users with 
	keeping track of their unit's life and up to one other resource (e.g. mana) as
	well. By doing this, it helps the programmer to:
		1: 	Reduce lines of code and complexity in projects.
		2: 	Allow developers freedom to creatively select which layout they
			would like, while at the same time keeping the visual integrity
			that is appealing.
		3:	Allow players to keep track of their health and secondary visually,
			thus improving game flow without unneeded attention dedicated to
			keeping track of some gameplay statistics.

	==========================================================================
	==========================================================================
	NOTES:
	- Portrait's resolution 5x7

	==========================================================================
	==========================================================================
	DOCUMENTATION:
	- 1: A full documentation.
	- 2: A history of versions.


	==========================================================================
	==========================================================================
	- SECTION 1

	Public Variables:
		bar_layout_position 			-	sets the position of the objects on the screen.
	 	max_health 						- 	sets the player's maximum health.
 		max_secondary					-	sets the player's maximum second attribute.
 		percent_width_of_healthbar		-	the percentage of the screen that the health bar is wide.
 		percent_height_of_healthbar		-	the percentage of the screen that the health bar is tall.
 		secondary_enabled				- 	is the player's second attribute available.
	 	secondary_is_shield				-	secondary is used as a shield and drained before health.
		unit_portrait_visible 			-	determines whether the unit's portrait is visible.
		unit_portrait 					-	the image used in the unit's portrait.

	Available Functions:
		full_health( time )				- sets unit's health to full over 'time' seconds.
		modify_health( amount )			- increments health by 'amount' immediately.
		modify_health( amount, time )	- increments health by 'amount' over 'time' seconds
		full_secondary( time )			- sets unit's secondary trait to full over 'time' seconds.
		modify_secondary( amount )		- increments secondary trait by 'amount' immediately
		modify_secondary( amount, time)	- increments secondary trait by 'amount' over 'time' seconds.

	** In addition, functions have been modified to work with secondary_is_shield enabled. **
	* to increment or restore shield, utilize modify_secondary() with a positive 'amount'
	* to increment or restore health, utilize modify_health() with a positive 'amound'

	==========================================================================
	==========================================================================
	- SECTION 2

 	VERSION 1.0	-
	For v1.0, I implemented a health bar and a secondary attribute bar. I also built a function
	that would allow the user to take damage and another to allow a player to heal (along with matching
	functions for the secondary attribute). Both bars took up a set amount of space and were located
	in the bottom left corner.

	=============================================================
	VERSION 1.1 -
	For v1.1, I first modified the functions to scale either bar up and down over time, while also leaving
	the ability to increment/decrement without the use of time. I then allowed the user to place the bars in any of
	the four corners of the screen. Additionally, I added labels to the health bar and secondary bar, allowing
	the player to both visually and cognitively recognize how much health and secondary they have remaining.

	=============================================================
	VERSION 1.2 - 
	For v1.2, I incorporated the center position on both the top and the bottom. In addition I added the
	ability to select all these positions based on just an enumeration, making it very simple for the user
	to select exactly which position they want the health bar to be located in. Finally, functionality was
	added that made the secondary attribute bar scale with the health bar's size, and also the functionality
	for the user to change the size of the health bar. This further allowed the users to take some creative
	liberties with how they set up their status bars while at the same time keeping the design integrity
	intact and the object functional.

	=============================================================
	VERSION 1.3 -
	For v1.3, I first added the ability to use the secondary attribute as a shield instead of another resource
	to spend. This means that the shield is drained before the health, and in addition, when health is added,
	it is added to the health bar and then to the shield. Also, the ability to insert a unit portrait was added.
	By simply attaching the image and setting a boolean, the selected character's portrait can be displayed.
	Originally, the portrait was located in the corner next to the status bars and bumped them over, however
	I later modified it to be displayed behind the status bars as I feel it is a cleaner look.

	========================================================================================================================
	========================================================================================================================
*/





public class PlayerManager : Photon.MonoBehaviour {
	// ===================================================================================================
	// enumerations used to determine behavior.
	//
	#region ENUMS

	public enum Position { TOP_LEFT, TOP_CENTER, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_CENTER, BOTTOM_RIGHT };
	public enum HorizontalOptions { LEFT, CENTER, RIGHT };
	public enum VerticalOptions { TOP, BOTTOM };

	#endregion





	// ===================================================================================================
	// PUBIC VARIABLES able to be set by player.
	//
	#region PUBLIC_VARIABLES
	public Position bar_layout_position = Position.TOP_LEFT; // sets the position of the objects on the screen.
	
	public float max_health = 0; // sets the player's maximum health.

	public float max_secondary = 0; // sets the player's maximum second attribute.

	public float percent_width_of_healthbar = 0.4f; // the percentage of the screen that the health bar is wide.

	public float percent_height_of_healthbar = 0.05f; // the percentage of the screen that the health bar is tall.

	public bool secondary_enabled = false; // is the player's second attribute available.
	
	public bool secondary_is_shield = false; // secondary is used as a shield and drained before health.

	public bool unit_portrait_visible = false; // determines whether the unit's portrait is visible.

	public Texture unit_portrait = null; // the image used in the unit's portrait.

	#endregion





	// ===================================================================================================
	// PUBLIC FUNCTIONS AVAILABLE TO THE USER;

	#region PUBLIC_FUNCTIONS
	// full_health(time)
	public void full_health(float time){ StartCoroutine( refreshHealth(time, 0) ); }

	// modify_health(health)
	[RPC]
	public void modify_health(float amount)
	{ 
		if(playerData.collectHitData)
		{
			GA.API.Design.NewEvent("Hit", this.transform.position);
		}

		modify_health(amount, -1); 
	}

	// modify_health(health, time)
	public void modify_health(float amount, float time)
	{
		if( !secondary_is_shield ) StartCoroutine( refreshHealth(time, amount) );
		else 
		{
			if( amount >= 0 ) StartCoroutine( refreshHealth(time, amount) );
			else if((cur_secondary2+amount) > 0 && amount < 0) StartCoroutine( refreshSecondary(time, amount) );
			else
			{
				if(amount + cur_secondary2 != 0) StartCoroutine( refreshHealth(time, amount + cur_secondary2 ) );
				if(cur_secondary2 != 0) StartCoroutine( refreshSecondary(time, -cur_secondary2) );
			}
		}
	}

	// full_secondary(time)
	public void full_secondary(float time){ StartCoroutine( refreshSecondary(time, 0) ); }

	// modify_secondary(health)
	public void modify_secondary(float amount){ modify_secondary(amount, -1); } 

	// modify_secondary(health, time)
	public void modify_secondary(float amount, float time)
	{
		if( !secondary_is_shield ) StartCoroutine( refreshSecondary(time, amount) );
		else
		{
			if((cur_secondary2+amount) > 0) StartCoroutine( refreshSecondary(time, amount) );
			else
			{
				if( amount + cur_secondary2 != 0 ) StartCoroutine( refreshHealth(time, amount + cur_secondary2 ) );
				if(cur_secondary2 != 0) StartCoroutine( refreshSecondary(time, -cur_secondary2) );
			}
		}
	}

	#endregion

	// ===================================================================================================
	// ===================================================================================================
	// ===================================================================================================
	// ===================================================================================================










	// ===================================================================================================
	// private variables that are not controlled by the player.
	//
	#region PRIVATE_VARIABLES
	private Texture life_bg, life_g, life_y, life_r, life_dr, secondary_b;
	private float health_bar_width, health_bar_height, cur_health, cur_health2, cur_secondary, cur_secondary2, margin_horizontal, margin_vertical;
	private HorizontalOptions healthbar_horizontal_location;
	private VerticalOptions healthbar_vertical_location;
	private Vector2 unit_portrait_position, unit_portrait_size;
	private SpawnPlayer spawnScript;
	private PlayerDataScript playerData;
	#endregion
	// ===================================================================================================
	// ===================================================================================================







	// ===================================================================================================
	// draws the objects to the screen.
	//
	#region ON_GUI
	void OnGUI(){

		// draws the unit portrait if it is on the screen.
		if(unit_portrait_visible)
		{
			GUI.DrawTexture(new Rect(unit_portrait_position.x, unit_portrait_position.y, unit_portrait_size.x + 6, unit_portrait_size.y + 6), life_bg);
			GUI.DrawTexture(new Rect(unit_portrait_position.x+3, unit_portrait_position.y+3,unit_portrait_size.x, unit_portrait_size.y), unit_portrait, ScaleMode.StretchToFill);
		}


		// draws the health bar and it's background.
		GUI.DrawTexture(new Rect(margin_horizontal-2,margin_vertical-2,health_bar_width+4,health_bar_height+4), life_bg);
		float f = (float) cur_health/max_health;
		if( f > 0.7 )
			GUI.DrawTexture( new Rect(margin_horizontal, margin_vertical,health_bar_width*f,health_bar_height), life_g);
		else if( f <= 0.7 && f > 0.4 )
			GUI.DrawTexture( new Rect(margin_horizontal,margin_vertical,health_bar_width*f,health_bar_height), life_y);
		else if( f <= 0.4 && f > 0.1 )
			GUI.DrawTexture( new Rect(margin_horizontal,margin_vertical,health_bar_width*f,health_bar_height), life_r);
		else if( f <= 0.1 )
			GUI.DrawTexture( new Rect(margin_horizontal,margin_vertical,health_bar_width*f,health_bar_height), life_dr);
		
		// label in health bar
		GUIStyle c = new GUIStyle();
		c.alignment = TextAnchor.MiddleCenter;
		c.fontStyle = FontStyle.Bold;
		c.normal.textColor = Color.gray;
		GUI.Label (new Rect(margin_horizontal-2,margin_vertical-2,health_bar_width+4,health_bar_height+4), Mathf.FloorToInt(cur_health) + " / " + Mathf.FloorToInt(max_health), c);
		
		
		// if second attribute is enabled, draw it.
		if(secondary_enabled)
		{
			float mar_vert = margin_vertical + health_bar_height;
			float mar_horz = margin_horizontal;
			if(healthbar_horizontal_location == HorizontalOptions.CENTER) mar_horz = ((Screen.width - (0.75f*health_bar_width)) / 2);
			
			GUI.DrawTexture(new Rect(mar_horz - 2, mar_vert, 0.75f*health_bar_width+4, 0.5f*health_bar_height+4), life_bg);
			GUI.DrawTexture( new Rect(mar_horz, mar_vert+2,0.75f*health_bar_width*cur_secondary/max_secondary, 0.5f*health_bar_height), secondary_b);
			
			// secondary label;
			GUIStyle c2 = new GUIStyle(c);
			c2.fontSize = c.fontSize/2;
			GUI.Label (new Rect(mar_horz - 2, mar_vert, 0.75f*health_bar_width+4, 0.5f*health_bar_height), "<size=10>" + Mathf.FloorToInt(cur_secondary) + " / " + Mathf.FloorToInt(max_secondary) + "</size>", c2);
			
		}
		

		
	}
	#endregion

	// ===================================================================================================
	// ===================================================================================================



	// ===================================================================================================
	// Start - Used for initialization
	//
	#region START
	private void Start () {
		
		if( bar_layout_position == Position.TOP_LEFT ){ healthbar_horizontal_location = HorizontalOptions.LEFT; healthbar_vertical_location = VerticalOptions.TOP; }
		else if( bar_layout_position == Position.TOP_CENTER ){ healthbar_horizontal_location = HorizontalOptions.CENTER; healthbar_vertical_location = VerticalOptions.TOP; }
		else if( bar_layout_position == Position.TOP_RIGHT ){ healthbar_horizontal_location = HorizontalOptions.RIGHT; healthbar_vertical_location = VerticalOptions.TOP; }
		else if( bar_layout_position == Position.BOTTOM_LEFT ){ healthbar_horizontal_location = HorizontalOptions.LEFT; healthbar_vertical_location = VerticalOptions.BOTTOM; }
		else if( bar_layout_position == Position.BOTTOM_CENTER ){ healthbar_horizontal_location = HorizontalOptions.CENTER; healthbar_vertical_location = VerticalOptions.BOTTOM; }
		else { healthbar_horizontal_location = HorizontalOptions.RIGHT; healthbar_vertical_location = VerticalOptions.BOTTOM; }
		
		// initialize secondary and health to starting at max;
		if( max_health == 0 ) max_health = 200;
		if( max_secondary == 0 ) max_secondary = 200;

		if( !secondary_enabled ) max_secondary = 0;

		cur_secondary = cur_secondary2 = max_secondary;
		cur_health = cur_health2 = max_health;
		
		health_bar_width = percent_width_of_healthbar * Screen.width;
		health_bar_height = percent_height_of_healthbar * Screen.height;
		
		life_bg = Resources.Load("PM_Assets/life-background") as Texture;
		life_g = Resources.Load("PM_Assets/life-green") as Texture;
		life_y = Resources.Load("PM_Assets/life-yellow") as Texture;
		life_r = Resources.Load("PM_Assets/life-red") as Texture;
		life_dr = Resources.Load("PM_Assets/life-dark-red") as Texture;
		secondary_b = Resources.Load("PM_Assets/secondary-blue") as Texture;
		
		if(healthbar_vertical_location == VerticalOptions.TOP) margin_vertical = 0.02f * Screen.height;
		else margin_vertical = (1 - (1.5f * percent_height_of_healthbar + 0.02f)) * Screen.height;
		
		if(healthbar_horizontal_location == HorizontalOptions.LEFT) margin_horizontal = 20;
		else if (healthbar_horizontal_location == HorizontalOptions.CENTER) margin_horizontal = (Screen.width - health_bar_width) / 2;
		else margin_horizontal = Screen.width - health_bar_width - 20;
		
		if( unit_portrait_visible )
		{
			if( unit_portrait == null ) unit_portrait = Resources.Load("PM_Assets/portrait-default") as Texture;

			unit_portrait_size.x = 0.15f * Screen.width;
			unit_portrait_size.y = unit_portrait_size.x * 1.4f;

			if( bar_layout_position == Position.TOP_LEFT || bar_layout_position == Position.TOP_CENTER){ unit_portrait_position.x = 10; unit_portrait_position.y = 10; }
			else if( bar_layout_position == Position.TOP_RIGHT ){ unit_portrait_position.x = Screen.width - unit_portrait_size.x - 10; unit_portrait_position.y = 10; }
			else if( bar_layout_position == Position.BOTTOM_LEFT || bar_layout_position == Position.BOTTOM_CENTER){ unit_portrait_position.x = 10; unit_portrait_position.y = Screen.height - unit_portrait_size.y - 10; }
			else { unit_portrait_position.x = Screen.width - unit_portrait_size.x - 10; unit_portrait_position.y = Screen.height - unit_portrait_size.y - 10; }

		}
		
		spawnScript = GameObject.Find("Spawner").GetComponent<SpawnPlayer>();
		playerData = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>();
	}
	#endregion
	// ===================================================================================================
	// ===================================================================================================
	



	
	
	
	// ===================================================================================================
	// Update is called once per frame
	//
	#region UPDATE
	private void Update () {
		if(cur_health > max_health) cur_health = max_health;
		else if(cur_health < 0) cur_health = 0;

		if(cur_secondary > max_secondary) cur_secondary = max_secondary;
		else if(cur_secondary < 0) cur_secondary = 0;



		if(cur_health == 0)
		{
			if(playerData.collectDeathData)
			{
				GA.API.Design.NewEvent("Death", this.transform.position);
			}
			spawnScript.spawn();
			PhotonNetwork.Destroy(this.gameObject);
		}




	}
	#endregion
	// ===================================================================================================
	// ===================================================================================================





	// ===================================================================================================
	// private functions, user does not use these.
	#region PRIVATE_FUNCTIONS
	private IEnumerator refreshHealth(float time, float health = 0)
	{
		if(cur_health2 != cur_health) cur_health = cur_health2;
		float start = cur_health2;

		if( health == 0 ) cur_health2 = max_health;
		else cur_health2 += health;

		if(cur_health2 < 0) cur_health2 = 0;
		else if(cur_health2 > max_health) cur_health2 = max_health;

		// if health not defined then max.
		float end = cur_health2;

		if(time > 0)
		{
			float i = 0.0f;
			float rate = 1.0f/time;
			while (i < 1.0f) {
				i += Time.deltaTime * rate;
				cur_health = Mathf.Lerp(start, end, i);
				yield return 0;
			}
		}
		else cur_health = end;
	}

	private IEnumerator refreshSecondary(float time, float amount = 0)
	{
		if(cur_secondary2 != cur_secondary) cur_secondary = cur_secondary2;
		float start = cur_secondary2;
		
		if( amount == 0 ) cur_secondary2 = max_secondary;
		else cur_secondary2 += amount;

		if(cur_secondary2 < 0) cur_secondary2 = 0;
		else if(cur_secondary2 > max_secondary) cur_secondary2 = max_secondary;
		
		// if health not defined then max.
		float end = cur_secondary2;
		
		if(time > 0)
		{
			float i = 0.0f;
			float rate = 1.0f/time;
			while (i < 1.0f) {
				i += Time.deltaTime * rate;
				cur_secondary = Mathf.Lerp(start, end, i);
				yield return 0;
			}
		}
		else cur_secondary = end;
	}
	#endregion
	// ===================================================================================================
	// ===================================================================================================









	
}
