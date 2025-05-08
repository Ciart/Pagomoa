using System;
using System.Reflection;
using UnityEngine;

public interface IManager {}

public class Manager<T> : IManager where T : Manager<T> 
{
    [Obsolete("Game.Instance.[Name]을 사용해주세요.")]
    public static T instance { get; private set; }
    
    public Manager()
    {
        instance ??= this as T;
        
        var type = GetType();
        var ms = ManagerSystem.Instance;
        
        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        foreach (MethodInfo method in methods)
        {
            if (method.DeclaringType == type && method.GetBaseDefinition().DeclaringType != type)
            {
                var action = (Action)Delegate.CreateDelegate(typeof(Action), this, method);
                switch (method.Name)
                {
                    case NameOfPreStart:
                        ms.preStart += action;
                        break;
                    case NameOfStart:
                        ms.start += action;
                        break;
                    case NameOfQuit:
                        ms.quit += action;
                        break;
                    case NameOfPreUpdate:
                        ms.preUpdate += action;
                        break;
                    case NameOfUpdate:
                        ms.update += action;
                        break;
                    case NameOfPreFixedUpdate:
                        ms.preFixedUpdate += action;
                        break;
                    case NameOfFixedUpdate:
                        ms.fixedUpdate += action;
                        break;
                    case NameOfPreLateUpdate:
                        ms.preLateUpdate += action;
                        break;
                    case NameOfLateUpdate:
                        ms.lateUpdate += action;
                        break;  
                }
            }
        }
    }
    
    public virtual void PreStart() { }
    public virtual void Start() { }
    public virtual void Quit() { }
    public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PreFixedUpdate () { }
    public virtual void FixedUpdate() { }
    public virtual void PreLateUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void OnDestroy() { }
    
    private const string NameOfPreStart = "PreStart";
    private const string NameOfStart = "Start";
    private const string NameOfQuit = "Quit";
    private const string NameOfPreUpdate = "PreUpdate";
    private const string NameOfUpdate = "Update";
    private const string NameOfPreFixedUpdate = "PreFixedUpdate";
    private const string NameOfFixedUpdate = "FixedUpdate";
    private const string NameOfPreLateUpdate = "PreLateUpdate";
    private const string NameOfLateUpdate = "LateUpdate";
}
