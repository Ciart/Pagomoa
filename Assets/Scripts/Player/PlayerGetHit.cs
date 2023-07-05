using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Player;
using UnityEngine;

public class PlayerGetHit : MonoBehaviour
{
    public bool isInvisible = false;
    [SerializeField] private float _invisibleCooltime = 2f;
    private Status _status;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public IEnumerator InvincibleCool()
    {
        isInvisible = true;
        
        yield return new WaitForSeconds(_invisibleCooltime);

        isInvisible = false;
    }
}
