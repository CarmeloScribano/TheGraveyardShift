using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// ----- Low Poly FPS Pack Free Version -----
public class AK47 : Gun {

	[Header("Weapon Settings")]
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

	[Header("Classes")]
	public Prefabs prefabs;
	public SoundClips soundClips;
	public SpawnPoints spawnPoints;

    private void Update()
    {
		Shoot(this.fireRate, this.bulletForce, this.mainAudioSource, this.shootAudioSource, this.soundClips, this.prefabs, this.spawnPoints, this.muzzleflashLight, this.lightDuration);
    }

}
// ----- Low Poly FPS Pack Free Version -----