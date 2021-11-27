using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Events;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shotSound = null;
    [SerializeField] private Transform forwardForward = null;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float bulletOffsetMultiplier = 2f;
    [SerializeField] private UnityEvent onMelee;

    private AudioSource audioSource = null;
    private float nextShootTime = 0f;
    private EnemyControls EC;
    private EntityHealth playerEntityHealth = null;

    bool previousFrame;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (forwardForward == null) forwardForward = transform;

        EC = this.gameObject.GetComponent<EnemyControls>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerEntityHealth = player.GetComponent<EntityHealth>();
    }

    void Update()
    {
        if(EC.canShootPlayer && !previousFrame)
        {
            nextShootTime = Time.time + cooldown;
        }
        previousFrame = EC.canShootPlayer;
        if (EC.canShootPlayer)
        {
            if (Time.time < nextShootTime) return;
            GameObject bulletClone = LeanPool.Spawn(bulletPrefab, PlanetManager.instance.GetPlanet().transform, true);
            bulletClone.transform.position = transform.position + forwardForward.forward * bulletOffsetMultiplier;
            bulletClone.transform.rotation = forwardForward.rotation;

            nextShootTime = Time.time + cooldown;

            if (shotSound != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(shotSound);
            }
        }       
    }
    private void OnCollisionStay(Collision collision)
    {
        if (playerEntityHealth != null && collision.transform == playerEntityHealth.transform && Time.time > nextShootTime)
        {
            nextShootTime = Time.time + cooldown;
            playerEntityHealth.DealDamage(1);
            onMelee.Invoke();
        }
    }
}
