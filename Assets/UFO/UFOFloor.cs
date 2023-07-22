using System;
using System.Collections;
using System.Collections.Generic;
using UFO;
using Unity.VisualScripting;
using UnityEngine;

public class UFOFloor : MonoBehaviour
{
    private EdgeCollider2D _edgeCollider2D;
    private CircleCollider2D inCollider;
    private CircleCollider2D outCollider;

    private UFOGateInteraction _gateInteraction;
    
    void Start()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        inCollider = transform.GetChild(0).GetChild(2).GetComponent<CircleCollider2D>();
        outCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();

        _gateInteraction = transform.GetChild(0).GetComponent<UFOGateInteraction>();
    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        if ( _gateInteraction.getKey && other.transform.name == "Player" )
        {
            /*if ( other.transform.position.y < _edgeCollider2D.transform.position.y)
            {
                _edgeCollider2D.enabled = false;
                outCollider.enabled = false;
                inCollider.enabled = false;
                
                StartCoroutine(DisableFloor());
            }*/
        }
    }
    
    private IEnumerator DisableFloor()
    {
        yield return new WaitForSeconds(1f);
        
        _edgeCollider2D.enabled = true;
        outCollider.enabled = true;
        inCollider.enabled = true;
    }
}
// orderinlayer로 플레이어 등장하듯이
// 