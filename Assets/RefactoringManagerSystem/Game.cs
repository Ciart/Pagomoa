using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

public class ManagerSystem : SingletonMonoBehaviour<ManagerSystem>
{
    public Action awake;
    public Action start;
    public Action quit;
    public Action preUpdate;
    public Action update;
    public Action preFixedUpdate;
    public Action fixedUpdate;
    public Action preLateUpdate;
    public Action lateUpdate;

    protected override void Awake()
    {
        awake?.Invoke();
    }

    private void Start()
    {
       start.Invoke();
       Debug.Log("Game::Start()");
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
