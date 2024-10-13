using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Logger;
using UnityEngine;


public class PGameInstance : MonoBehaviour
{
    private static PGameInstance instance = null;

    private static QuestManager _questManagerInstance; 
    public static QuestManager questManager => questManager;
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        if(Managers == null)
        {
            Managers = new List<PManager>();

            foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType().IsSubclassOf(typeof(PManager)))
                {
                    PManager manager = Activator.CreateInstance(assembly.GetType()) as PManager;
                    Managers.Add(manager);
                }
            }
        }

        foreach (PManager mgr in Managers)
        {
            mgr.PreAwake();
        }

        foreach (PManager mgr in Managers)
        {
            mgr.Awake();
        }

        foreach (PManager mgr in Managers)
        {
            mgr.PostAwake();
        }
    }


   private void Start()
   {
        if (Managers != null)
        {
            foreach (PManager manager in Managers)
            {
                manager.Start();
            }
        }
    }

   // Update is called once per frame
   private void Update()
   {
        if(Managers != null)
        {
            foreach (PManager manager in Managers)
            {
                if(manager.GetNeedToPreUpdate())
                {
                    manager.PreUpdate();
                }
            }
         
            foreach (PManager manager in Managers) 
            {
                if(manager.GetNeedToUpdate())
                {
                    manager.Update();
                }
            }

            foreach (PManager manager in Managers)
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
        if (Managers != null)
        {
             foreach (PManager manager in Managers)
             {
                if(manager.GetNeedToPreFixedUpdate())
                {
                    manager.PreFixedUpdate();
                }
             }
             
             foreach (PManager manager in Managers)
             {
                if(manager.GetNeedToFixedUpdate())
                {
                    manager.FixedUpdate();
                }

             }

             foreach (PManager manager in Managers) 
             {
                if(manager.GetNeedToPostFixedUpdate())
                {
                    manager.PostFixedUpdate();
                }
             }
        }
    }

    private List<PManager> Managers = null;
}