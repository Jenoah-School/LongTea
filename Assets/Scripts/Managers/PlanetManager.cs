using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager instance = null;

    private GameObject currentPlanet;

    public void Awake()
    {
        instance = this;
        SetPlanet(GameObject.FindGameObjectWithTag("Planet"));
    }

    public void SetPlanet(GameObject planet)
    {
        if (planet != null)
        {
            currentPlanet = planet;
        }
    }

    public GameObject GetPlanet()
    {
        return currentPlanet;
    }
}
