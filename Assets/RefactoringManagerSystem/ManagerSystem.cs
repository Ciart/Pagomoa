using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

public class ManagerSystem : SingletonMonoBehaviour<ManagerSystem>
{
    public Action preStart;
    public Action start;
    public Action quit;
    public Action preUpdate;
    public Action update;
    public Action preFixedUpdate;
    public Action fixedUpdate;
    public Action preLateUpdate;
    public Action lateUpdate;

    private void Start()
    {
        preStart?.Invoke();
        
        start?.Invoke();
    }

    // Update is called once per frame
    private void Update()
    {
        preUpdate?.Invoke();

        update?.Invoke();
    }

    private void FixedUpdate()
    {
        preFixedUpdate?.Invoke();

        fixedUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        preLateUpdate?.Invoke();

        lateUpdate?.Invoke();
    }

    private void OnApplicationQuit()
    {
        quit?.Invoke();
    }
}