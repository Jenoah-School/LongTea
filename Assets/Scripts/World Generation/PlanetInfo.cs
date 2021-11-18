using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    [SerializeField] private float planetSize = 5f;

    public void SetPlanetSize(float newPlanetSize)
    {
        planetSize = newPlanetSize;
    }

    public float GetPlanetSize()
    {
        return planetSize;
    }
}
