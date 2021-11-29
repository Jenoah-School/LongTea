using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet Biome", menuName = "Planet Generation/Planet Biome", order = 100)]
public class PlanetBiome : ScriptableObject
{
    public Gradient heightGradient = new Gradient();
    [Range(0f, 2f)] public float noiseScale = 0.4f;

    [Header("Spawn objects")]
    public List<PlanetObject> enemies = new List<PlanetObject>();
    public int enemyCount = 3;
    [Space(20)]
    public int vegetationAmount = 20;
    public List<PlanetObject> vegetation = new List<PlanetObject>();
    [Space(20)]
    public int noColliderAmount = 20;
    public List<PlanetObject> noColliderObjects = new List<PlanetObject>();
    [Space(20)]
    public int resourceAmount = 4;
    public List<PlanetObject> resources = new List<PlanetObject>();
    [Space(20)]
    public int mustSpawnedItemAmount = 1;
    public List<PlanetObject> mustSpawnedItems = new List<PlanetObject>();
}
