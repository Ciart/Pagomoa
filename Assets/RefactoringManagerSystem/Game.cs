using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game _instance = null;
    private static List<PManager> _managers = null;

    public Action awake;
    public Action start;
    public Action preUpdate;
    public Action update;
    public Action preFixedUpdate;
    public Action fixedUpdate;
    public Action preLateUpdate;
    public Action lateUpdate;
    
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
            _managers = new List<PManager>();
            
            foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(PManager)))
                    {
                        PManager manager = Activator.CreateInstance(type) as PManager;

                        if (manager != null)
                        {
                            _managers.Add(manager);
                            manager.Init(this);   
                        }
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
}