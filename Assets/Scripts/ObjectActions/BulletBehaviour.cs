using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using DG.Tweening;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float planetSize = 5f;
    [SerializeField] private AudioClip destroySound = null;

    private PlanetInfo planetInfo;

    private void Start()
    {
        planetInfo = PlanetManager.instance.GetPlanet();
        planetSize = planetInfo.GetPlanetSize();
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
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
        Transform collidedTransform = collision.collider.transform;
        if (collidedTransform.CompareTag("Enemy"))
        {
            collidedTransform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
            Destroy(collidedTransform.gameObject, .2f);
        }
        LeanPool.Despawn(gameObject);
    }
}
