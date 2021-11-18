using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

[RequireComponent(typeof(AudioSource))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shotSound = null;

    private AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bulletClone = LeanPool.Spawn(bulletPrefab, null, true);
            bulletClone.transform.position = transform.position + transform.forward;
            bulletClone.transform.rotation = transform.rotation;

            if (shotSound != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(shotSound);
            }
        }
    }
}
