using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    //Animator component attached to weapon
    Animator anim;

	public enum FireWeaponSettings
    {
		Automatic,
		Semi
    }

    [Header("Gun Camera")]
    //Main gun camera
    public Camera gunCamera;

    [Header("Gun Camera Options")]
    //How fast the camera field of view changes when aiming 
    [Tooltip("How fast the camera field of view changes when aiming.")]
    public float fovSpeed = 15.0f;
    //Default camera field of view
    [Tooltip("Default value for camera field of view (40 is recommended).")]
    public float defaultFov = 20.0f;

    public float aimFov = 10.0f;

    [Header("UI Weapon Name")]
    [Tooltip("Name of the current weapon, shown in the game UI.")]
    public string weaponName;
	private string storedWeaponName;

	[Header("Weapon Sway")]
    //Enables weapon sway
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

	[Header("UI Components")]
	//public Text timescaleText;
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;

	private Vector3 initialSwayPosition;

	private bool soundHasPlayed = false;

	//Used for fire rate
	private float lastFired;
	[Header("Weapon Settings")]
	//How fast the weapon fires, higher value means faster rate of fire
	//Eanbles auto reloading when out of ammo
	[Tooltip("Enables auto reloading when out of ammo.")]
	public bool autoReload;
	//Delay between shooting last bullet and reloading
	public float autoReloadDelay;
	//Check if reloading
	private bool isReloading;

	//Holstering weapon
	//private bool hasBeenHolstered = false;

	//If weapon is holstered
	//private bool holstered;
	//Check if running
	private bool isRunning;
	//Check if aiming
	private bool isAiming;
	//Check if walking
	private bool isWalking;
	//Check if inspecting weapon
	private bool isInspecting;

	//How much ammo is currently left
	private int currentAmmo;
	//Totalt amount of ammo
	[Tooltip("How much ammo the weapon should have.")]
	public int ammo;
	//Check if out of ammo
	private bool outOfAmmo;

	[Header("Muzzleflash Settings")]
	public bool randomMuzzleflash = false;
	//min should always bee 1
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

	private void Start()
    {

		//GameObject canvas = GameObject.FindGameObjectWithTag("HUD");
		currentWeaponText = GameObject.FindGameObjectWithTag("WeaponName").GetComponent<Text>();
		currentAmmoText = GameObject.FindGameObjectWithTag("WeaponAmmo").GetComponent<Text>();
		totalAmmoText = GameObject.FindGameObjectWithTag("WeaponTotalAmmo").GetComponent<Text>();

		//Save the weapon name
		storedWeaponName = weaponName;
        //Get weapon name from string to text
        currentWeaponText.text = weaponName;
        //Set total ammo text from total ammo int
        totalAmmoText.text = ammo.ToString();

		//Weapon sway
		initialSwayPosition = transform.localPosition;
    }

	private void Awake()
	{

		//Set the animator component
		anim = GetComponent<Animator>();
		//Set current ammo to total ammo value
		currentAmmo = ammo;

		//muzzleflashLight.enabled = false;
	}

	private void LateUpdate()
	{

		//Weapon sway
		if (weaponSway == true)
		{
			float movementX = -Input.GetAxis("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
			//Clamp movement to min and max values
			movementX = Mathf.Clamp
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp
				(movementY, -maxSwayAmount, maxSwayAmount);
			//Lerp local pos
			Vector3 finalSwayPosition = new Vector3
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp
				(transform.localPosition, finalSwayPosition +
					initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}

	public void Shoot(float fireRate, float bulletForce, AudioSource mainAudioSource, AudioSource shootAudioSource, SoundClips soundClips, Prefabs prefabs, SpawnPoints spawnPoints, Light muzzleflashLight, float lightDuration)
    {
		//Aiming
		//Toggle camera FOV when right click is held down
		if (Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting)
		{

			isAiming = true;
			//Start aiming
			anim.SetBool("Aim", true);

			//When right click is released
			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
				aimFov, fovSpeed * Time.deltaTime);

			if (!soundHasPlayed)
			{
				mainAudioSource.clip = soundClips.aimSound;
				mainAudioSource.Play();

				soundHasPlayed = true;
			}
		}
		else
		{
			//When right click is released
			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
				defaultFov, fovSpeed * Time.deltaTime);

			isAiming = false;
			//Stop aiming
			anim.SetBool("Aim", false);

			soundHasPlayed = false;
		}
		//Aiming end

		//If randomize muzzleflash is true, genereate random int values
		if (randomMuzzleflash == true)
		{
			randomMuzzleflashValue = Random.Range(minRandomValue, maxRandomValue);
		}

		//Set current ammo text from ammo int
		currentAmmoText.text = currentAmmo.ToString();

		//Continosuly check which animation 
		//is currently playing
		AnimationCheck();

		//Play knife attack 1 animation when Q key is pressed
		if (Input.GetKeyDown(KeyCode.Q) && !isInspecting)
		{
			anim.Play("Knife Attack 1", 0, 0f);
		}
		//Play knife attack 2 animation when F key is pressed
		if (Input.GetKeyDown(KeyCode.F) && !isInspecting)
		{
			anim.Play("Knife Attack 2", 0, 0f);
		}

		//Throw grenade when pressing G key
		//if (Input.GetKeyDown (KeyCode.G) && !isInspecting) 
		//{
		//	StartCoroutine (GrenadeSpawnDelay ());
		//	//Play grenade throw animation
		//	anim.Play("GrenadeThrow", 0, 0.0f);
		//}

		//If out of ammo
		if (currentAmmo == 0)
		{
			//Show out of ammo text
			currentWeaponText.text = "OUT OF AMMO";
			//Toggle bool
			outOfAmmo = true;
			//Auto reload if true
			//if (autoReload == true && !isReloading)
			//{
			//	StartCoroutine(AutoReload());
			//}
		}
		else
		{
			//When ammo is full, show weapon name again
			currentWeaponText.text = storedWeaponName.ToString();
			//Toggle bool
			outOfAmmo = false;
			//anim.SetBool ("Out Of Ammo", false);
		}

		//AUtomatic fire
		//Left click hold 
		if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning)
		{
			//Shoot automatic
			if (Time.time - lastFired > 1 / fireRate)
			{
				lastFired = Time.time;

				//Remove 1 bullet from ammo
				currentAmmo -= 1;

				shootAudioSource.clip = soundClips.shootSound;
				shootAudioSource.Play();

				if (!isAiming) //if not aiming
				{
					anim.Play("Fire", 0, 0f);
					//If random muzzle is false
					if (!randomMuzzleflash &&
						enableMuzzleflash == true)
					{
						muzzleParticles.Emit(1);
						//Light flash start
						StartCoroutine(MuzzleFlashLight(muzzleflashLight, lightDuration));
					}
					else if (randomMuzzleflash == true)
					{
						//Only emit if random value is 1
						if (randomMuzzleflashValue == 1)
						{
							if (enableSparks == true)
							{
								//Emit random amount of spark particles
								sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
							}
							if (enableMuzzleflash == true)
							{
								muzzleParticles.Emit(1);
								//Light flash start
								StartCoroutine(MuzzleFlashLight(muzzleflashLight, lightDuration));
							}
						}
					}
				}
				else //if aiming
				{

					anim.Play("Aim Fire", 0, 0f);

					//If random muzzle is false
					if (!randomMuzzleflash)
					{
						muzzleParticles.Emit(1);
						//If random muzzle is true
					}
					else if (randomMuzzleflash == true)
					{
						//Only emit if random value is 1
						if (randomMuzzleflashValue == 1)
						{
							if (enableSparks == true)
							{
								//Emit random amount of spark particles
								sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
							}
							if (enableMuzzleflash == true)
							{
								muzzleParticles.Emit(1);
								//Light flash start
								StartCoroutine(MuzzleFlashLight(muzzleflashLight, lightDuration));
							}
						}
					}
				}

				//Spawn bullet from bullet spawnpoint
				var bullet = (Transform)Instantiate(
					prefabs.bulletPrefab,
					spawnPoints.bulletSpawnPoint.transform.position,
					spawnPoints.bulletSpawnPoint.transform.rotation);

				//Add velocity to the bullet
				bullet.GetComponent<Rigidbody>().velocity =
					bullet.transform.forward * bulletForce;

				//Spawn casing prefab at spawnpoint
				Instantiate(prefabs.casingPrefab,
					spawnPoints.casingSpawnPoint.transform.position,
					spawnPoints.casingSpawnPoint.transform.rotation);
			}
		}
	}

	private void Reload(SoundClips soundClips, AudioSource mainAudioSource, SkinnedMeshRenderer bulletInMagRenderer, float showBulletInMagDelay)
	{

		if (outOfAmmo == true)
		{
			//Play diff anim if out of ammo
			anim.Play("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = soundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null)
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer>().enabled = false;
				//Start show bullet delay
				StartCoroutine(ShowBulletInMag(showBulletInMagDelay, bulletInMagRenderer));
			}
		}
		else
		{
			//Play diff anim if ammo left
			anim.Play("Reload Ammo Left", 0, 0f);

			mainAudioSource.clip = soundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play();

			//If reloading when ammo left, show bullet in mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null)
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer>().enabled = true;
			}
		}
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	public void UpdateMethods(SoundClips soundClips, AudioSource mainAudioSource, SkinnedMeshRenderer bulletInMagRenderer, float showBulletInMagDelay)
	{
		//Reload 
		if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting)
		{
			//Reload
			Reload(soundClips, mainAudioSource, bulletInMagRenderer, showBulletInMagDelay);
		}

		//Walking when pressing down WASD keys
		if (Input.GetKey(KeyCode.W) && !isRunning ||
			Input.GetKey(KeyCode.A) && !isRunning ||
			Input.GetKey(KeyCode.S) && !isRunning ||
			Input.GetKey(KeyCode.D) && !isRunning)
		{
			anim.SetBool("Walk", true);
		}
		else
		{
			anim.SetBool("Walk", false);
		}

		//Running when pressing down W and Left Shift key
		if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)))
		{
			isRunning = true;
		}
		else
		{
			isRunning = false;
		}

		//Run anim toggle
		if (isRunning == true)
		{
			anim.SetBool("Run", true);
		}
		else
		{
			anim.SetBool("Run", false);
		}
	}

	//private IEnumerator GrenadeSpawnDelay () {

	//	//Wait for set amount of time before spawning grenade
	//	yield return new WaitForSeconds (grenadeSpawnDelay);
	//	//Spawn grenade prefab at spawnpoint
	//	Instantiate(Prefabs.grenadePrefab, 
	//		Spawnpoints.grenadeSpawnPoint.transform.position, 
	//		Spawnpoints.grenadeSpawnPoint.transform.rotation);
	//}

	//private IEnumerator AutoReload()
	//{
	//	//Wait set amount of time
	//	yield return new WaitForSeconds(autoReloadDelay);

	//	if (outOfAmmo == true)
	//	{
	//		//Play diff anim if out of ammo
	//		anim.Play("Reload Out Of Ammo", 0, 0f);

	//		mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
	//		mainAudioSource.Play();

	//		//If out of ammo, hide the bullet renderer in the mag
	//		//Do not show if bullet renderer is not assigned in inspector
	//		if (bulletInMagRenderer != null)
	//		{
	//			bulletInMagRenderer.GetComponent
	//			<SkinnedMeshRenderer>().enabled = false;
	//			//Start show bullet delay
	//			StartCoroutine(ShowBulletInMag());
	//		}
	//	}
	//	//Restore ammo when reloading
	//	currentAmmo = ammo;
	//	outOfAmmo = false;
	//}

	//Reload
	private void Reload(AudioSource mainAudioSource, SoundClips soundClips, SkinnedMeshRenderer bulletInMagRenderer, float showBulletInMagDelay)
	{

		if (outOfAmmo == true)
		{
			//Play diff anim if out of ammo
			anim.Play("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = soundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null)
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer>().enabled = false;
				//Start show bullet delay
				StartCoroutine(ShowBulletInMag(showBulletInMagDelay, bulletInMagRenderer));
			}
		}
		else
		{
			//Play diff anim if ammo left
			anim.Play("Reload Ammo Left", 0, 0f);

			mainAudioSource.clip = soundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play();

			//If reloading when ammo left, show bullet in mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null)
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer>().enabled = true;
			}
		}
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	//Enable bullet in mag renderer after set amount of time
	private IEnumerator ShowBulletInMag(float showBulletInMagDelay, SkinnedMeshRenderer bulletInMagRenderer)
	{

		//Wait set amount of time before showing bullet in mag
		yield return new WaitForSeconds(showBulletInMagDelay);
		bulletInMagRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
	}

	//Show light when shooting, then disable after set amount of time
	private IEnumerator MuzzleFlashLight(Light muzzleflashLight, float lightDuration)
	{

		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds(lightDuration);
		muzzleflashLight.enabled = false;
	}

	//Check current animation playing
	private void AnimationCheck()
	{

		//Check if reloading
		//Check both animations
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo") ||
			anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"))
		{
			isReloading = true;
		}
		else
		{
			isReloading = false;
		}

		//Check if inspecting weapon
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Inspect"))
		{
			isInspecting = true;
		}
		else
		{
			isInspecting = false;
		}
	}

}
