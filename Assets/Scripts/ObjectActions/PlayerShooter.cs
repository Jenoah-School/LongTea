using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bulletOrigin = new GameObject("bulletOrigin");
            bulletOrigin.transform.parent = PlanetManager.instance.GetPlanet().transform;
            bulletOrigin.transform.position = PlanetManager.instance.GetPlanet().transform.position;

            bulletOrigin.transform.forward = transform.forward;

            GameObject bulletClone = LeanPool.Spawn(bullet, bulletOrigin.transform, true);
            bulletClone.transform.position = transform.position + transform.forward * 2;
            bulletClone.transform.rotation = transform.rotation;

        }
    }
}
