using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AK47 : Gun {

    private void Update()
    {
		UpdateMethods();
    }

    private void FixedUpdate()
    {
        Shoot();
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