using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityToPlanet : MonoBehaviour
{
    [SerializeField] private Transform planet;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool rotateToPlanet = true;

    private Rigidbody rb = null;
    private bool hasInitialized = false;

    void Start()
    {
        planet ??= PlanetManager.instance.GetPlanet().transform;
        rb = GetComponent<Rigidbody>();
        Invoke("Initialize", 0.2f);
    }

    void Initialize()
    {
        if(planet == null)
        {
            planet = PlanetManager.instance.GetPlanet().transform;
        }
    }

    void FixedUpdate()
    {
        if (planet == null) return;

        Vector3 gravityDirection = (planet.position - transform.position).normalized;

        rb.AddForce(gravityDirection * gravity, ForceMode.Acceleration);

        if (rotateToPlanet)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50f * Time.deltaTime);
        }
    }
}
