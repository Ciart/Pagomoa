internal class PManager
{
    public virtual void PreAwake() {}
    public virtual void Awake() { }
    public virtual void PostAwake() {}
    public virtual void Start() { }

    private bool NeedToPreUpdate = false;
    private bool NeedToUpdate = false;
    private bool NeedToPostUpdate = false;

    private bool NeedToPreFixedUpdate = false;
    private bool NeedToFixedUpdate = false;
    private bool NeedToPostFixedUpdate = false;

    public bool GetNeedToPreUpdate() {  return NeedToPreUpdate; }
    public bool GetNeedToUpdate() { return NeedToUpdate; }
    public bool GetNeedToPostUpdate() { return NeedToPostUpdate; }

    public bool GetNeedToPreFixedUpdate() {  return NeedToPreFixedUpdate; }
    public bool GetNeedToFixedUpdate() { return NeedToFixedUpdate; }
    public bool GetNeedToPostFixedUpdate() { return NeedToPostFixedUpdate; }

    public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PostUpdate() { }
    public virtual void PreFixedUpdate () { }
    public virtual void FixedUpdate() { }
    public virtual void PostFixedUpdate() { }
    public virtual void OnDestroy() { }
}