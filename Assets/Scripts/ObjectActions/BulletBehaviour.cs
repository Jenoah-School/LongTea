using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AudioClip destroySound = null;

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.RotateAround(PlanetManager.instance.GetPlanet().transform.position, transform.right, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.gameObject.SetActive(false);
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
        LeanPool.Despawn(gameObject);
    }
}
