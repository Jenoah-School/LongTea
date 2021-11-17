using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.parent.transform.localRotation *= Quaternion.Euler(Vector3.right * Time.deltaTime * 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.gameObject.SetActive(false);
    }
}
