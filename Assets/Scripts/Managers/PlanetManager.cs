using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager instance = null;

    private PlanetInfo currentPlanet;

    public void Awake()
    {
        instance = this;
        SetPlanet(GameObject.FindGameObjectWithTag("Planet"));
    }

    public void SetPlanet(PlanetInfo planet)
    {
        if (planet != null)
        {
            currentPlanet = planet;
        }
    }

    public void SetPlanet(GameObject planet)
    {
        if (planet != null)
        {
            planet.TryGetComponent(out currentPlanet);
        }
    }

    public PlanetInfo GetPlanet()
    {
        return currentPlanet;
    }

    public float GetRadius()
    {
        return 1f;
    }
}
