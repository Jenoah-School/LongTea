using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] public float health = 3f;
    [SerializeField] private UnityEvent OnHit;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private bool hasPartialImmunity = false;
    [SerializeField] private float cooldownTime = 1f;

    protected bool isDead = false;
    protected float startHealth = 3f;

    private float nextHitTime = 0f;

    private void Start()
    {
        startHealth = health;
        nextHitTime = Time.time;
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            OnDeath.Invoke();
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    public virtual void DealDamage(float damageAmount)
    {
        //Skip if entity is already dead
        if (isDead || (hasPartialImmunity && Time.time < nextHitTime)) return;
        nextHitTime = Time.time + cooldownTime;

        health -= damageAmount;
        if (health <= 0)
        {
            OnDeath.Invoke();
            isDead = true;
        }
        else
        {
            OnHit.Invoke();
        }
    }
}
