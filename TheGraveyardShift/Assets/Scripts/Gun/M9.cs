using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class M9 : Gun
{
	private void Update()
	{
		//Shoot(fireRate,
		//	bulletForce,
		//	mainAudioSource,
		//	shootAudioSource,
		//	soundClips,
		//	prefabs,
		//	spawnPoints,
		//	muzzleflashLight,
		//	lightDuration);

		//UpdateMethods(soundClips, mainAudioSource, bulletInMagRenderer, showBulletInMagDelay);

		Shoot();

		UpdateMethods();
	}
}
// ----- Low Poly FPS Pack Free Version -----