using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: look into refractoring/removing duplication between this and meleeattack
[RequireComponent(typeof(EventController))]
public class ProjectileAttack : MonoBehaviour
{
    public float damage = 1f;
    public int numToPool = 10;
    public float releaseForce = 100f;
    public float maxProjTimeAlive = 2f;
    public Transform spawnLocation;
    public Transform hitTarget;
    public Projectile prefab;

    private EventController events;
    private ObjectPool<Projectile> projPool;

    void Awake()
    {
        events = GetComponent<EventController>();
    }

    void Start()
    {
        projPool = new ObjectPool<Projectile>(prefab, numToPool);
    }

    public void PerformProjectileAttack()
    {
        var proj = projPool.Get();

        var dir = (hitTarget.position - spawnLocation.position).normalized;

        proj.transform.position = spawnLocation.position;
        proj.gameObject.SetActive(true);
        proj.Initialise(projPool, events, damage, dir * releaseForce, maxProjTimeAlive);
    }
}
