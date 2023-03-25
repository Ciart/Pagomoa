using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLight : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.transform.name == "Tilemap")
        {
            Destroy(gameObject);
        }
    }
}
