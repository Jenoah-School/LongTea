using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerHealth : MonoBehaviour
{
    private EntityHealth playerEntityHealth;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(!player.TryGetComponent(out playerEntityHealth))
        {
            enabled = false;
        }
    }

    public void AddHealth(int healthIncrementAmount)
    {
        playerEntityHealth.DealDamage(-healthIncrementAmount);
    }
}
