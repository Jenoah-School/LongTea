using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet Biome", menuName = "Planet Generation/Planet Biome", order = 100)]
public class PlanetBiome : ScriptableObject
{
    public Material planetMaterial;
    [Range(0f, 2f)] public float noiseScale = 0.4f;
    [Space(20)]
    public int vegetationAmount = 20;
    public List<PlanetObject> vegetation = new List<PlanetObject>();
    [Space(20)]
    public int noColliderAmount = 20;
    public List<PlanetObject> noColliderObjects = new List<PlanetObject>();
    [Space(20)]
    public int resourceAmount = 4;
    public List<PlanetObject> resources = new List<PlanetObject>();
}
