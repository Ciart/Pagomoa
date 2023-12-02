using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinTimeUI : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private Transform _image;

    private void Start()
    {
        _image = GetComponent<Transform>();
        StartCoroutine(RotateUI());
    }

    private IEnumerator RotateUI()
    {
        while (true)
        {
            _image.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime * 360f);
            yield return null;
        }
    }
}
