using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PManager
{
    public void Init(Game game)
    {
        var type = GetType();
        
        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            if (method.DeclaringType == type && method.GetBaseDefinition().DeclaringType != type)
            {
                var action = (Action)Delegate.CreateDelegate(typeof(Action), this, method);
                switch (method.Name)
                {
                    case NameOfAwake:
                        game.awake += action;
                        break;
                    case NameOfStart:
                        game.start += action;
                        break;
                    case NameOfPreUpdate:
                        game.preUpdate += action;
                        break;
                    case NameOfUpdate:
                        game.update += action;
                        break;
                    case NameOfPreFixedUpdate:
                        game.preFixedUpdate += action;
                        break;
                    case NameOfFixedUpdate:
                        game.fixedUpdate += action;
                        break;
                    case NameOfPreLateUpdate:
                        game.preLateUpdate += action;
                        break;
                    case NameOfLateUpdate:
                        game.lateUpdate += action;
                        break;  
                }

                Debug.Log("Overridden Method: " + method.Name);
            }
        }
    }
    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PreFixedUpdate () { }
    public virtual void FixedUpdate() { }
    public virtual void PreLateUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void OnDestroy() { }
    
    private const string NameOfAwake = "Awake";
    private const string NameOfStart = "Start";
    private const string NameOfPreUpdate = "PreUpdate";
    private const string NameOfUpdate = "Update";
    private const string NameOfPreFixedUpdate = "PreFixedUpdate";
    private const string NameOfFixedUpdate = "FixedUpdate";
    private const string NameOfPreLateUpdate = "PreLateUpdate";
    private const string NameOfLateUpdate = "LateUpdate";
}
