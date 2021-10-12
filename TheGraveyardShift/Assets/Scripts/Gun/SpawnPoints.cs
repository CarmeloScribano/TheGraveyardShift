using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
	[Header("Spawnpoints")]
	//Array holding casing spawn points 
	//(some weapons use more than one casing spawn)
	//Casing spawn point array
	public Transform casingSpawnPoint;
	//Bullet prefab spawn from this point
	public Transform bulletSpawnPoint;
	public Transform grenadeSpawnPoint;
}
