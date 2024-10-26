using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game _instance = null;

    public static T Get<T>() where T : PManager
    {
        foreach (PManager manager in _managers)
        {
            if (manager.GetType() == typeof(T))
            {
                return manager as T;
            }
        }
        
        // T 타입에 해당하는 매니저가 없는 경우 null 반환
        Debug.LogWarning($"No manager of type {typeof(T)} found.");
        return null;
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
            _managers = new List<PManager>();
            
            foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(PManager)))
                    {
                        PManager manager = Activator.CreateInstance(type) as PManager;
                        _managers.Add(manager);
                    } 
                }
            }
        }

        IsSingleton();
        
        foreach (PManager mgr in _managers)
        {
            mgr.PreAwake();
        }
        
        foreach (PManager mgr in _managers)
        {
            mgr.Awake();
        }
        
        foreach (PManager mgr in _managers)
        {
            mgr.PostAwake();
        }
    }


   private void Start()
   {
        if (_managers != null)
        {
            foreach (PManager manager in _managers)
            {
                manager.Start();
            }
        }
    }

   // Update is called once per frame
   private void Update()
   {
        if(_managers != null)
        {
            foreach (PManager manager in _managers)
            {
                if(manager.GetNeedToPreUpdate())
                {
                    manager.PreUpdate();
                }
            }
         
            foreach (PManager manager in _managers) 
            {
                if(manager.GetNeedToUpdate())
                {
                    manager.Update();
                }
            }

            foreach (PManager manager in _managers)
            { 
                if(manager.GetNeedToPostUpdate())
                {
                    manager.PostUpdate();
                }
            }
        }
   }

    private void FixedUpdate()
    {
        if (_managers != null)
        {
             foreach (PManager manager in _managers)
             {
                if(manager.GetNeedToPreFixedUpdate())
                {
                    manager.PreFixedUpdate();
                }
             }
             
             foreach (PManager manager in _managers)
             {
                if(manager.GetNeedToFixedUpdate())
                {
                    manager.FixedUpdate();
                }

             }

             foreach (PManager manager in _managers) 
             {
                if(manager.GetNeedToPostFixedUpdate())
                {
                    manager.PostFixedUpdate();
                }
             }
        }
    }

    private void OnDestroy()
    {
        foreach (PManager manager in _managers)
        {
            manager.OnDestroy();
        }
    }

    private static List<PManager> _managers = null;

    private void IsSingleton()
    {

        for (int i = _managers.Count - 1; i >= 0; i--)
        {
            var list = _managers[i].CheckSingleton(_managers);
            if (list != null)
            {
                _managers = list;
            }
        }
        
        
    }
}