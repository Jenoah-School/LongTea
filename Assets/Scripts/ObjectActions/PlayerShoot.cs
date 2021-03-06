using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shotSound = null;
    [SerializeField] private Transform forwardForward = null;
    [SerializeField] private Joystick shootingJoystick;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float bulletOffsetMultiplier = 1f;
    [SerializeField] private UnityEvent onShoot;

    private AudioSource audioSource = null;
    private float nextShootTime = 0f;

    private bool wasDownLastTouch = false;
    private bool canShoot = false;

    Rigidbody rb;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        if(forwardForward == null) forwardForward = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (shootingJoystick != null)
        {
            canShoot = shootingJoystick.Direction.magnitude > 0;

            if (canShoot && !wasDownLastTouch)
            {
                nextShootTime = Time.time + cooldown;
            }

            wasDownLastTouch = canShoot;
        }

        if (Input.GetKey(KeyCode.Space) || (shootingJoystick != null && canShoot))
        {
            if (Time.time < nextShootTime) return;

            GameObject bulletClone = LeanPool.Spawn(bulletPrefab, PlanetManager.instance.GetPlanet().transform, true);
            bulletClone.transform.rotation = forwardForward.rotation;
            bulletClone.transform.position = transform.position + bulletClone.transform.forward + rb.velocity * bulletOffsetMultiplier;
            BulletBehaviour bulletBehaviour = bulletClone.GetComponent<BulletBehaviour>();
            bulletBehaviour.SetTargetDirection();
            bulletBehaviour.trailRenderer.Clear();

            nextShootTime = Time.time + cooldown;

            if (shotSound != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(shotSound);
            }

            onShoot.Invoke();
        }

        
    }
}
