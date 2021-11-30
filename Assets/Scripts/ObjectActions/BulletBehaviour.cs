using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using DG.Tweening;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float planetSize = 5f;
    
    [SerializeField] public TrailRenderer trailRenderer = null;

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound = null;

    private PlanetInfo planetInfo;

    private void Start()
    {
        planetInfo = PlanetManager.instance.GetPlanet();
        planetSize = planetInfo.GetPlanetSize();
    }

    private void OnEnable()
    {
        trailRenderer.Clear();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.RotateAround(PlanetManager.instance.GetPlanet().transform.position, transform.right, (moveSpeed * Time.deltaTime) / planetSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //other.gameObject.SetActive(false);
        

        Transform collidedTransform = collision.collider.transform;

        if(collidedTransform.TryGetComponent(out EntityHealth entityHealth))
        {
            entityHealth.DealDamage(1);
        }
        else if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        LeanPool.Despawn(gameObject);
    }
}
