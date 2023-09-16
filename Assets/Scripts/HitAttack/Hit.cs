using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hit : MonoBehaviour
{
    bool unbeatable = false;
    public float unbeatTime = 0.3f;
    public bool IsHitTarget()
    {
        return !unbeatable;
    }
    public void OnHit(GameObject attacker, float damage)
    {
        //Debug.Log($"{gameObject.name} : {attacker.name}로부터 {damage}의 피해를 입었음!");
        switch (gameObject.tag)
        {
            case "Player":
                GetComponent<Player.PlayerController>().GetDamage(attacker, damage);
                break;
            case "Monster":
                GetComponent<Monster>().GetDamage(attacker, damage);
                break;
        }
        StartCoroutine("UnBeatable", unbeatTime);
    }
    IEnumerator UnBeatable(float time)
    {
        unbeatable = true;
        yield return new WaitForSeconds(unbeatTime);
        unbeatable = false;
    }
}
