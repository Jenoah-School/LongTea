using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    [SerializeField] private float planetSize = 5f;
    [SerializeField] private int resourceAmount = 0;

    public void SetPlanetSize(float newPlanetSize)
    {
        planetSize = newPlanetSize;
    }

    public void SetResourceAmount(int newResourceAmount)
    {
        resourceAmount = newResourceAmount;
    }

    public float GetPlanetSize()
    {
        return planetSize;
    }

    public int GetResourceAmount()
    {
        return resourceAmount;
    }
}
