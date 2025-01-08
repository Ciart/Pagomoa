using System;
using System.Reflection;

public interface IPManager {}

public class PManager<T> : IPManager where T : PManager<T> 
{
    public PManager()
    {
        var type = GetType();
        var ms = ManagerSystem.instance;
        
        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            if (method.DeclaringType == type && method.GetBaseDefinition().DeclaringType != type)
            {
                var action = (Action)Delegate.CreateDelegate(typeof(Action), this, method);
                switch (method.Name)
                {
                    case NameOfAwake:
                        ms.awake += action;
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

    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Quit() { }
    public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PreFixedUpdate () { }
    public virtual void FixedUpdate() { }
    public virtual void PreLateUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void OnDestroy() { }
    
    private const string NameOfAwake = "Awake";
    private const string NameOfStart = "Start";
    private const string NameOfQuit = "Quit";
    private const string NameOfPreUpdate = "PreUpdate";
    private const string NameOfUpdate = "Update";
    private const string NameOfPreFixedUpdate = "PreFixedUpdate";
    private const string NameOfFixedUpdate = "FixedUpdate";
    private const string NameOfPreLateUpdate = "PreLateUpdate";
    private const string NameOfLateUpdate = "LateUpdate";
}
