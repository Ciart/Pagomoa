using System;
using System.Reflection;

public interface IPManager
{
    public void Init(GameSystem game);
}

public class PManager<T> : IPManager where T : PManager<T> 
{
    public static T instance { get; private set; }

    protected PManager()
    {
        instance ??= this as T;
    }
    
    public void Init(GameSystem game)
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
                    case NameOfQuit:
                        game.quit += action;
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
