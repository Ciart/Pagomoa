using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Constants;

[RequireComponent(typeof(Attack))]
public class PlayerAttack : MonoBehaviour
{
    bool attack = false;
    int attackDirection = 0;

    PlayerController _playerController;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            attack = true;
    }
    private void FixedUpdate()
    {
        AttackDirectionDetect();
        if (attack)
        {
            NormatAttack();
            attack = false;
        }
    }
    void AttackDirectionDetect()
    {
        switch (_playerController.GetDirection()) {
            case Direction.Right:
                attackDirection = 1;
                break;
            case Direction.Left:
                attackDirection = -1;
                break;
        }
    }
    void NormatAttack()
    {
        Debug.Log("�⺻����!" + gameObject.name);

        Vector3 pointA, pointB, playerPosition = _playerController.transform.position;

        pointA = playerPosition + new Vector3(0 * attackDirection, 0.5f);
        pointB = playerPosition + new Vector3(1.5f * attackDirection, -0.5f);

        GetComponent<Attack>()._Attack(gameObject, GetComponent<Status>().attackpower, pointA, pointB, 1);

    }
}
