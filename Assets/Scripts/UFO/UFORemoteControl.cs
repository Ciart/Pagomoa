using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worlds;

public class UFORemoteControl : MonoBehaviour
{
    public InherentItem inherentItem;
    
    public InherentItem UFORemote
    {
        get => inherentItem;
        
        set => inherentItem = value;
    }
}
