using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Attack : MonoBehaviour
{
    public List<string> attackTargetTag;
    public void _Attack(GameObject attacker, float damage, Vector3 startPoint, Vector3 endPoint, int canHitCount = 1)
    {
        int count = canHitCount;
        Collider2D[] deffenders = Physics2D.OverlapAreaAll(startPoint, endPoint);
        foreach (Collider2D deffender in deffenders)
        {
            Hit target = deffender.GetComponent<Hit>();

            if (!target) continue;
            if (deffender.isTrigger) continue;
            if (!isAttackTarget(target.gameObject)) continue;
            if (!target.IsHitTarget()) continue;

            target.OnHit(attacker, damage);
        }
        
    }
    public void _Attack(GameObject attacker, GameObject deffender, float damage)
    {
        Hit target = deffender.GetComponent<Hit>();

        if (!target) return;
        if (deffender.GetComponent<Collider2D>().isTrigger) return;
        if (!isAttackTarget(target.gameObject)) return;
        if (!target.IsHitTarget()) return;

        target.OnHit(attacker, damage);
    }
    bool isAttackTarget(GameObject target)
    {
        return attackTargetTag.Contains(target.tag) ? true : false;
    }
}
