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
        SetPlanet();
    }

    public void SetPlanet()
    {
        currentPlanet = GameObject.Find("Planet");
    }

    public GameObject GetPlanet()
    {
        return currentPlanet;
    }
}
