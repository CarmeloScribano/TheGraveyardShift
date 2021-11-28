using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    #region Fields
    public enum FireWeaponSettings
	{
		Automatic,
		Semi
	}

    //Animator component attached to weapon
    Animator anim;

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

	[Header("Weapon Settings")]
	[Tooltip("How the gun shoots.")]
	public FireWeaponSettings fireType;
	//How fast the weapon fires, higher value means faster rate of fire
	[Tooltip("How fast the weapon fires, higher value means faster rate of fire.")]
	public float fireRate;
	[Header("Bullet Settings")]
	//Bullet
	[Tooltip("How much force is applied to the bullet when shooting.")]
	public float bulletForce = 400.0f;
	[Tooltip("How long after reloading that the bullet model becomes visible " +
		"again, only used for out of ammo reload animations.")]
	public float showBulletInMagDelay = 0.6f;
	[Tooltip("The bullet model inside the mag, not used for all weapons.")]
	public SkinnedMeshRenderer bulletInMagRenderer;

	[Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

	[Header("Audio Source")]
	//Main audio source
	public AudioSource mainAudioSource;
	//Audio source used for shoot sound
	public AudioSource shootAudioSource;

	[Header("Grenade Settings")]
	public float grenadeSpawnDelay = 0.35f;

	[Header("Classes")]
	public Prefabs prefabs;
	public SoundClips soundClips;
	public SpawnPoints spawnPoints;

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
	private Text currentWeaponText;
	private Text currentAmmoText;
	private Text totalAmmoText;
	private Image currentWeaponIcon;
	public Sprite WeaponIcon;

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
	public int maxAmmo;
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

	private int totalAmmo;

	[SerializeField] private float bulletRange = 50f;
	[SerializeField] private float damage = 30f;
	private float knifeRange = 2f;
	private float knifeDamage = 50f;

	private float knifeDelay = 0.5f;
	private bool canKnife = true;

	[Header("Impact Effect Prefabs")]
	public Transform[] bloodImpactPrefabs;

	#endregion

	public void OnAwake()
	{
		//Set the animator component
		anim = GetComponent<Animator>();
		//Set current ammo to total ammo value
		currentAmmo = ammo;

		//muzzleflashLight.enabled = false;
	}

	public void OnStart()
    {
		FindHUDElements();

		TakeOut();

		totalAmmo = ammo;

		//GameObject canvas = GameObject.FindGameObjectWithTag("HUD");

		//Save the weapon name
		storedWeaponName = weaponName;
        //Get weapon name from string to text
        currentWeaponText.text = weaponName;
        //Set total ammo text from total ammo int
        totalAmmoText.text = maxAmmo.ToString();

		//Weapon sway
		initialSwayPosition = transform.localPosition;
    }
	
	private void FindHUDElements()
    {
		currentWeaponText = GameObject.FindGameObjectWithTag("WeaponName").GetComponent<Text>();
		currentAmmoText = GameObject.FindGameObjectWithTag("WeaponAmmo").GetComponent<Text>();
		totalAmmoText = GameObject.FindGameObjectWithTag("WeaponTotalAmmo").GetComponent<Text>();
		currentWeaponIcon = GameObject.FindGameObjectWithTag("WeaponIcon").GetComponent<Image>();
	}

	public void OnLateUpdate()
    {
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

	public virtual void OnWeaponUse()
	{
		FindHUDElements();

		//if (currentWeaponIcon != null)
		  //  {
		currentWeaponIcon.sprite = WeaponIcon;
		 //}

		totalAmmoText.text = maxAmmo.ToString();

		TakeOut();
	}

	public void Shoot()
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

		Knife();

		//Grenade();

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

		Shooting();
	}

	private void TakeOut()
    {
		mainAudioSource.clip = soundClips.takeOutSound;
		mainAudioSource.Play();
	}

	private void Shooting()
    {
		//AUtomatic fire
		//Left click hold 
		if (fireType == FireWeaponSettings.Automatic)
		{
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
						CreateBullet();
						//If random muzzle is false
						if (!randomMuzzleflash &&
							enableMuzzleflash == true)
						{
							muzzleParticles.Emit(1);
							//Light flash start
							StartCoroutine(MuzzleFlashLight());
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
									StartCoroutine(MuzzleFlashLight());
								}
							}
						}
					}
					else //if aiming
					{

						anim.Play("Aim Fire", 0, 0f);
						CreateBullet();

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
									StartCoroutine(MuzzleFlashLight());
								}
							}
						}
					}
				}
			}
		}
		if (fireType == FireWeaponSettings.Semi){
			//Shooting 
			if (Input.GetMouseButtonDown(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning)
			{
				anim.Play("Fire", 0, 0f);

				muzzleParticles.Emit(1);

				//Remove 1 bullet from ammo
				currentAmmo -= 1;

				shootAudioSource.clip = soundClips.shootSound;
				shootAudioSource.Play();

				//Light flash start
				StartCoroutine(MuzzleFlashLight());

				if (!isAiming) //if not aiming
				{
					anim.Play("Fire", 0, 0f);
					CreateBullet();

					muzzleParticles.Emit(1);

					if (enableSparks == true)
					{
						//Emit random amount of spark particles
						sparkParticles.Emit(Random.Range(1, 6));
					}
				}
				else //if aiming
				{
					anim.Play("Aim Fire", 0, 0f);
					CreateBullet();

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
								sparkParticles.Emit(Random.Range(1, 6));
							}
							if (enableMuzzleflash == true)
							{
								muzzleParticles.Emit(1);
								//Light flash start
								StartCoroutine(MuzzleFlashLight());
							}
						}
					}
				}
			}
		}
	}

	private void Knife()
    {
		//Play knife attack 1 animation when Q key is pressed
		//if (Input.GetKeyDown(KeyCode.Q) && !isInspecting)
		//{
		//	anim.Play("Knife Attack 1", 0, 0f);
		//}
		//Play knife attack 2 animation when F key is pressed
		if (Input.GetKeyDown(KeyCode.V) && !isInspecting)
		{
			if (canKnife)
            {
				canKnife = false;
				anim.Play("Knife Attack 2", 0, 0f);
				StartCoroutine(DealKnifeDamage());
			}
		}
	}

	private IEnumerator DealKnifeDamage()
    {
		yield return new WaitForSeconds(0.3f);
		RaycastHit hit;

		CapsuleCollider collider = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>();

		Vector3 p1 = transform.position;

		// Cast a sphere wrapping character controller 10 meters forward
		// to see if it is about to hit anything.

		//transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity
		//if (Physics.Raycast(p1, transform.forward, out hit))
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
		{
			if (hit.transform.tag == "Enemy")
			{
                if (hit.distance < knifeRange)
                {
                    EnemyAI ai = hit.transform.gameObject.GetComponent<EnemyAI>();
					if (ai != null)
					{
						ai.TakeDamage(knifeDamage);

						//Instantiate random impact prefab from array
						Instantiate(bloodImpactPrefabs[Random.Range
							(0, bloodImpactPrefabs.Length)], hit.transform.position + new Vector3(0,0.9f,0),
							Quaternion.LookRotation(p1));
					}
				}

			}

		}

		yield return new WaitForSeconds(knifeDelay);

		canKnife = true;
	}

	private void Grenade()
    {
		////Throw grenade when pressing G key
		if (Input.GetKeyDown (KeyCode.G) && !isInspecting) 
		{
			StartCoroutine (GrenadeSpawnDelay ());
			//Play grenade throw animation
			anim.Play("GrenadeThrow", 0, 0.0f);
		}
	}

	private IEnumerator GrenadeSpawnDelay()
	{

		//Wait for set amount of time before spawning grenade
		yield return new WaitForSeconds(grenadeSpawnDelay);
		//Spawn grenade prefab at spawnpoint
		Instantiate(prefabs.grenadePrefab,
			spawnPoints.grenadeSpawnPoint.transform.position,
			spawnPoints.grenadeSpawnPoint.transform.rotation);
	}

	private void CastDamage()
    {
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if (Physics.Raycast(spawnPoints.bulletSpawnPoint.position, fwd, out hit, bulletRange))
        {
			if (hit.transform.tag == "Enemy")
            {
				EnemyAI ai = hit.transform.gameObject.GetComponent<EnemyAI>();
				if (ai != null)
                {
					ai.TakeDamage(damage);
                }
            }
        }
    }

	private void CreateBullet() {
		CastDamage();

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

	private void TextColorChange()
    {
		if (currentAmmo < (totalAmmo * 0.3f))
		{
			currentAmmoText.color = new Color(255, 0, 0);
		}
        else
        {
			currentAmmoText.color = new Color(255, 255, 255);
		}

	}

	private float GetAnimationTime(string clipName)
    {
		float cTime = 0f;

		AnimationClip[] arrclip = GetComponent<Animator>().runtimeAnimatorController.animationClips;
		foreach (AnimationClip clip in arrclip)
		{
			if (clip.name.Contains(clipName))
            {
				cTime = clip.length;
            }
		}

		return cTime;
    }

	private IEnumerator Reload()
	{

		if (currentAmmo < totalAmmo)
        {
			float reloadTime = 0f;

			if (outOfAmmo == true)
			{
				//Play diff anim if out of ammo
				anim.Play("Reload Out Of Ammo", 0, 0f);
				reloadTime = GetAnimationTime("reload_out_of_ammo");

				mainAudioSource.clip = soundClips.reloadSoundOutOfAmmo;
				mainAudioSource.Play();

				//If out of ammo, hide the bullet renderer in the mag
				//Do not show if bullet renderer is not assigned in inspector
				if (bulletInMagRenderer != null)
				{
					bulletInMagRenderer.GetComponent
					<SkinnedMeshRenderer>().enabled = false;
					//Start show bullet delay
					StartCoroutine(ShowBulletInMag());
				}
			}
			else
			{
				//Play diff anim if ammo left
				anim.Play("Reload Ammo Left", 0, 0f);
				reloadTime = GetAnimationTime("reload_ammo_left");

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

			yield return new WaitForSeconds(reloadTime);

			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Draw"))
			{
				//Restore ammo when reloading
				//if totalAmmo itself have enough bullets
				if (maxAmmo >= totalAmmo)
				{
					//get the ammo required to fill the magazine
					int neededAmmo = totalAmmo - currentAmmo;
					//decrement the ammo required to fill the magazine from total 
					maxAmmo -= neededAmmo;
					//then increment the ammo required to fill the magazine  
					currentAmmo += neededAmmo;
				}
				//if total ammo is less then the magazineCapacity
				else if (maxAmmo > 0)
				{
					//get the ammo required to fill the magazine
					int neededAmmo = ammo - currentAmmo;
					//if totalAmmo has enough bullets required to fill the magazine
					if (neededAmmo < maxAmmo)
					{
						maxAmmo -= neededAmmo;
						currentAmmo += neededAmmo;
					}
					//if totalAmmo is less then the required neededAmmo then incerement all the bullets in the currentAmmo from totalAmmo
					else
					{
						currentAmmo += maxAmmo;
						maxAmmo = 0;
					}
				}

				totalAmmoText.text = maxAmmo.ToString();
			}

			outOfAmmo = false;
		}
	}

	public void UpdateMethods()
	{
		//Reload 
		if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting)
		{
			//Reload
			StartCoroutine(Reload());
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

		TextColorChange();
	}

	private IEnumerator AutoReload()
	{
		//Wait set amount of time
		yield return new WaitForSeconds(autoReloadDelay);

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
				StartCoroutine(ShowBulletInMag());
			}
		}
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	private IEnumerator ShowBulletInMag()
	{

		//Wait set amount of time before showing bullet in mag
		yield return new WaitForSeconds(showBulletInMagDelay);
		bulletInMagRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
	}

	private IEnumerator MuzzleFlashLight()
	{

		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds(lightDuration);
		muzzleflashLight.enabled = false;
	}

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
