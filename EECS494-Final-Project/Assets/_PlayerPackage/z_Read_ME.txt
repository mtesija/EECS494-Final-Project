

Important Notes:

0]	Add the "MainMenu" and "Game" scene to the Build settings. File -> Build Settings -> Add Current.
	Set the Game Resolution to: 1400 x 800. File -> Build Settings -> Select Webplayer or PC, Mac, Linux. Press Player Settings -> Resolution Tab (1400 x 800). For the best experience.

1]	When building your own FPS character/player that works properly, use these Layer settings for the optimal gameplay experience:
	Layer 8: "GunFront"
	In the Hierarchy set these objects to these Layers:
	'Player' to "Ignore Raycast" 			-> If aiming down, the player wont shoot on itself ;).
	'Graphics' to "Ignore Raycast"
	'WeaponHolder' to 'Default'
	'CameraRecoilHolder' to 'Default'
	'GunRendererThroughObjects' to 'Default'		-> This object is a Camera which is being used to render your Weapon on front of all other pixels at all cost. Set Its Clear Flags to Depth Only, CullingMask to GunFront and Depth to 10.	
	'Main Camera' to 'Default'						-> Set depth to 1, so that the Above camera is rendered last, which will render the weapon on top of anything else. Culling mask to Anything but the GunFront layer.
	'WeaponWalkHolder' to 'Default'
	'WeaponJumpAnimation' to 'Default'
	'WeaponRecoilHolder' to 'Default'
	'ADSHolder' to 'Default'
	'SCAR-H' to 'GunFront'
	'Arm' to 'GunFront'
	'Barrel' to 'GunFront'
	'Barrel' to 'GunFront'
	'Laser' to 'Default'			-> Default because of the linerenderer rendering through anything else, and you want it to stop after hitting a surface.normal.
	'LaserCircle' to 'GunFront'

2]	[This Is Done for you] - We have used the standard Unity3D Character controller to put the gun on. Import this through: Assets -> Import Package -> Character Controller (First Person Character Controller).
	
3]	Assign a 'Run' button to the player. Edit -> Project Settings -> Input: 	
	Add a new Axes. Name that Axes Run, and Change the Positive Button to: 'left shift'

4]	Note that the gun will only work on surfaces with a collider attached to it. So when shooting at the Skybox, the gun wont trigger. Make sure to conseal every possible space while shooting.
		
5]	On the Camera Object "GunRendererThroughObjects" in the Hierarchy, Put its tag from "MainCamera" to "Untagged" (We are not using this Camera as the Main Camera). Else the Zoom Function wont work.

6]	Controls:
	W/A/S/D For Moving.
	Shift for Sprinting.
	Esc for the Pause Menu.
	M1 for Shooting.
	M2 to Zoom In.
	
	
	
	
We hope you enjoy this package and be able to learn alot from it!
If you have any questions, feel free to contact me through my personal e-mail address and I would be happy to help you out!

E: Wabo@live.nl