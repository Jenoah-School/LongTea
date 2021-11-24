using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shotSound = null;
    [SerializeField] private Transform forwardForward = null;
    [SerializeField] private float cooldown = 0.5f;

    private AudioSource audioSource = null;
    private float nextShootTime = 0f;

    private EnemyControls EC;

    bool previousFrame;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (forwardForward == null) forwardForward = transform;

        EC = this.gameObject.GetComponent<EnemyControls>(); 
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
            GameObject bulletClone = LeanPool.Spawn(bulletPrefab, null, true);
            bulletClone.transform.position = transform.position + forwardForward.forward;
            bulletClone.transform.rotation = forwardForward.rotation;

            nextShootTime = Time.time + cooldown;

            if (shotSound != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(shotSound);
            }
        }       
    }
}
