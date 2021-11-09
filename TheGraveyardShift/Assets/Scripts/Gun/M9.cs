using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class M9 : Gun
{
	private void FixedUpdate()
	{

		Shoot();

		UpdateMethods();
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