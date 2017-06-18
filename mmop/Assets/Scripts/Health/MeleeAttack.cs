using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float damage = 1f;
    public LayerMask hitMask;

    public Transform hitTarget;

    void Start()
    {
        Physics2D.queriesStartInColliders = false;    
    }

    public void PerformMeleeAttack()
    {


        var hit = Physics2D.Linecast(transform.position, hitTarget.position, hitMask);

        if(hit.collider != null)
        {
            var damageables = hit.collider.GetComponents<Damageable>();

            if (damageables.Length > 0)
            {
                Vector2 dir = (hitTarget.position - transform.position).normalized;

                foreach (var d in damageables)
                {
                    d.Damage(damage, dir);
                }
            }
        }
    }
}