using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private float bobDistance = 10f;
    [SerializeField] private float bobOffsetTime = 100f;

    private Vector3 startPosition = Vector3.zero;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = startPosition + (Vector3.up * bobDistance) * Mathf.Cos(Time.time * bobSpeed + bobOffsetTime);
    }
}
