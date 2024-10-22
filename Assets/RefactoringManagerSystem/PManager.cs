using System.Collections.Generic;
using UnityEngine;

public class PManager
{
    public virtual void PreAwake() {}
    public virtual void Awake() { }
    public virtual void PostAwake() {}
    public virtual void Start() { }

    protected bool needToPreUpdate = false;
    protected bool needToUpdate = false;
    protected bool needToPostUpdate = false;

    protected bool needToPreFixedUpdate = false;
    protected bool needToFixedUpdate = false;
    protected bool needToPostFixedUpdate = false;

    public bool GetNeedToPreUpdate() {  return needToPreUpdate; }
    public bool GetNeedToUpdate() { return needToUpdate; }
    public bool GetNeedToPostUpdate() { return needToPostUpdate; }

    public bool GetNeedToPreFixedUpdate() {  return needToPreFixedUpdate; }
    public bool GetNeedToFixedUpdate() { return needToFixedUpdate; }
    public bool GetNeedToPostFixedUpdate() { return needToPostFixedUpdate; }

    public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PostUpdate() { }
    public virtual void PreFixedUpdate () { }
    public virtual void FixedUpdate() { }
    public virtual void PostFixedUpdate() { }
    public virtual void OnDestroy() { }

    public List<PManager> CheckSingleton(List<PManager> managers)
    {
        foreach (var manager in managers)
        {
            if (manager.GetType() == GetType())
            {
                Debug.LogError(manager.GetType() + " " + GetType());

                managers.Remove(this);
                return managers;
            }
        }
        
        return null;
    }
}
