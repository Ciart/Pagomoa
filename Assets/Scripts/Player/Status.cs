using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] protected float _oxygen = 100;
    [SerializeField] protected float _max_oxygen = 100;
    [SerializeField] protected float _min_oxygen = 0;
    [SerializeField] protected float _hungry = 100;
    [SerializeField] protected float _max_hungry = 100;
    [SerializeField] protected float _min_hungry = 0;
    public float oxygen { get { return _oxygen; } set { _oxygen = value; } }
    public float max_oxygen { get { return _max_oxygen; } set { _max_oxygen = value; } }
    public float min_oxygen { get { return _min_oxygen; } set { _min_oxygen = value; } }
    public float hungry { get { return _hungry; } set { _hungry = value; } }
    public float max_hungry { get { return _max_hungry; } set { _max_hungry = value; } }
    public float min_hungry { get { return _min_hungry; } set { _min_hungry = value; } }
}
