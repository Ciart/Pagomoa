using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Worlds;

public class BombPrefab : MonoBehaviour
{
    [SerializeField] private float waitingTime;
    [SerializeField] private Sprite BombImage;

    private IEnumerator corutine;
    void Start()
    {
        corutine = Wait(3, gameObject);
        StartCoroutine(corutine);
    }
    private IEnumerator Wait(float WaitTime, GameObject Bomb)
    {
        yield return new WaitForSeconds(WaitTime);
        SetBombImage(Bomb);
        Destroy(Bomb, waitingTime);
    }
    private void SetBombImage(GameObject Bomb)
    {
        Bomb.GetComponent<SpriteRenderer>().sprite = BombImage;

        var point = transform.position + new Vector3(-2, -2.2f);
        
        for (int j = 0; j < 3; j++)
        {
            point.y += 1;
            for (int i = 0; i < 3; i++)
            {
                point.x += 1;
                var pointInt = WorldManager.ComputeCoords(point);
                WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999);
            }
            point.x = transform.position.x - 2;
        }
    }
    private void Destroy(GameObject Bomb, int time)
    {
        Destroy(Bomb, time);
    }
}
