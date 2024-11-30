using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game _instance = null;
    private static List<IPManager> _managers = null;

    public Action awake;
    public Action start;
    public Action quit;
    public Action preUpdate;
    public Action update;
    public Action preFixedUpdate;
    public Action fixedUpdate;
    public Action preLateUpdate;
    public Action lateUpdate;
    
    private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(_managers == null)
        {
            _managers = new List<IPManager>();
            
            foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (IsSubclassOfRawGeneric(typeof(PManager<>), type) 
                        && typeof(IPManager).IsAssignableFrom(type)
                        && !type.ContainsGenericParameters) // 구체적인 타입만 허용
                    {
                        var manager = Activator.CreateInstance(type) as IPManager;

                        _managers.Add(manager);
                        manager?.Init(this);
                    } 
                }
            }
        }
        
        awake?.Invoke();
    }


   private void Start()
   {
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
