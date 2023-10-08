using System.Collections;
using UnityEngine;
using Worlds;

public class BombPrefab : MonoBehaviour
{
    [SerializeField] private float _waitingTime;
    [SerializeField] private GameObject _bombImage;
    [SerializeField] private GameObject _bombEffect;


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
        Destroy(Bomb, _waitingTime);
    }
    private void SetBombImage(GameObject Bomb)
    {
        _bombImage.SetActive(false);
        _bombEffect.SetActive(true);
        Bomb.transform.localScale = new Vector3(10, 10, 1);
        var point = transform.position + new Vector3(-2, -1.2f);
        
        for (int j = 0; j < 3; j++)
        {
            point.y += 1;
            for (int i = 0; i < 3; i++)
            {
                point.x += 1;
                var pointInt = WorldManager.ComputeCoords(point);
                WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999, true);
            }
            point.x = transform.position.x - 2;
        }
    }
    private void Destroy(GameObject Bomb, int time)
    {
        Destroy(Bomb, time);
    }
}
