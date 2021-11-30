using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Pistol : Gun
{

	private void Update()
	{
		UpdateMethods();
		Shoot();
	}

    private void FixedUpdate()
    {
		
	}

    private void LateUpdate()
	{
		OnLateUpdate();
	}

	private void Start()
	{
		OnStart();
	}

	private void Awake()
	{
		OnAwake();
	}
}