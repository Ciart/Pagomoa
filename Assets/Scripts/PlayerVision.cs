using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerVision : MonoBehaviour
{
    [SerializeField] private SpriteMask _spriteMask;
    [SerializeField] private GameObject _fog;
    private Vector3 _prevPosition;

    void Start()
    {
        _prevPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != _prevPosition) 
        {
            _prevPosition = transform.position;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.GetComponent<CircleCollider2D>()) return;
        //_fog.GetComponent<FogManager>().fadeOutFogs( collision.GetComponent<CircleCollider2D>() );
    }
}

