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

    private Rigidbody rb;
    private SimpleAnimation SA;

    bool previousFrame;

    float attackPauseInterval = 1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (forwardForward == null) forwardForward = transform;

        EC = this.gameObject.GetComponent<EnemyControls>();

        rb = gameObject.GetComponent<Rigidbody>();
        SA = gameObject.GetComponent<EnemyControls>().simpleAnimation;

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

            StartCoroutine(OnShoot());

            GameObject bulletClone = LeanPool.Spawn(bulletPrefab, PlanetManager.instance.GetPlanet().transform, true);
            bulletClone.transform.position = transform.position + forwardForward.forward * bulletOffsetMultiplier;
            bulletClone.transform.rotation = forwardForward.rotation;
            bulletClone.GetComponent<BulletBehaviour>().trailRenderer.Clear();

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
        StartCoroutine(OnHit(collision));
    }

    IEnumerator OnShoot()
    {
        if(SA != null) SA.StopMovingImmediate();
        rb.isKinematic = true;
        yield return new WaitForSeconds(attackPauseInterval);
        GameObject bulletClone = LeanPool.Spawn(bulletPrefab, PlanetManager.instance.GetPlanet().transform, true);
        bulletClone.transform.position = transform.position + forwardForward.forward * bulletOffsetMultiplier;
        bulletClone.transform.rotation = forwardForward.rotation;

        if (shotSound != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(shotSound);
        }

        rb.isKinematic = false;
        if (SA != null) SA.StartMoving();
    }

    IEnumerator OnHit(Collision collision)
    {
        if (playerEntityHealth != null && collision.transform == playerEntityHealth.transform && Time.time > nextShootTime)
        {
            nextShootTime = Time.time + cooldown;
            playerEntityHealth.DealDamage(1);
            onMelee.Invoke();
            rb.isKinematic = true;
            yield return new WaitForSeconds(attackPauseInterval);
            rb.isKinematic = false;

        }
    }
}
