using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Events;

public class PlanetGeneration : MonoBehaviour
{
    [SerializeField] private int resolution = 5;
    [SerializeField] private float planetSize = 1f;
    [SerializeField] private bool flatShaded = true;
    [SerializeField] private string planetLayer = "Planet";
    [SerializeField] private Material vertexColorMaterial;
    [SerializeField] private float spawningSafezone = 4f;

    [Header("Finish")]
    [SerializeField] private UnityEvent onPlanetFinishedGeneration;

    [Header("Structures")]
    [SerializeField] private LayerMask structureIgnoreLayers;
    [SerializeField] private int spawnIterations = 4;
    [Space(20)]
    [SerializeField] private List<PlanetBiome> planetBiomes = new List<PlanetBiome>();

    [Header("Debug")]
    [SerializeField] private List<PlanetInfo> planets = new List<PlanetInfo>();
    [SerializeField] private List<GameObject> placedResources = new List<GameObject>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color32[] cubeUV;
    private Color32[] colors;

    private GameObject currentPlanet = null;
    private GameObject spawnPrevention = null;

    private Transform player = null;
    private PlanetBiome currentBiome = null;
    private Noise noise;

    private void Start()
    {
        noise = new Noise();
        if (planetBiomes.Count <= 0) return;
        currentBiome = planetBiomes[Random.Range(0, planetBiomes.Count)];
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPrevention = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spawnPrevention.name = "Spawning safezone";
        spawnPrevention.transform.localScale = Vector3.one * spawningSafezone;
        Generate();
    }

    public void DisableCurrentPlanet()
    {
        currentPlanet.SetActive(false);

        if (planetBiomes.Count <= 0) return;
        currentBiome = planetBiomes[Random.Range(0, planetBiomes.Count)];
        

        placedResources.Clear();
    }

    public void Generate()
    {
        currentPlanet = new GameObject("Planet #" + Random.Range(1000, 9999));

        currentPlanet.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
        MeshRenderer planetMeshRenderer = currentPlanet.AddComponent<MeshRenderer>();
        planetMeshRenderer.sharedMaterial = vertexColorMaterial;
        currentPlanet.layer = LayerMask.NameToLayer(planetLayer);

        mesh.name = "Planet #" + Random.Range(1000, 9999);

        CreateVertices();
        CreateTriangles();
        AddNoise();
        if (flatShaded) ShadeFlat();
        ApplyColor();


        PlanetInfo planetInfo = currentPlanet.AddComponent<PlanetInfo>();
        currentPlanet.AddComponent<SphereCollider>();
        planets.Add(planetInfo);
        planetInfo.SetPlanetSize(planetSize);

        enemies.Clear();
        placedResources.Clear();

        spawnPrevention.SetActive(true);
        Vector3 playerDirection = (player.position - transform.position).normalized;
        spawnPrevention.transform.position = transform.position + playerDirection * planetSize;

        StartCoroutine(SpawnObjects(currentBiome.resources, currentBiome.resourceAmount, placedResources, PlaceMustSpawns));

        PlaceColliderlessObjects();

        //TODO: Make this not regenerate on each planet generation
        PlanetManager.instance.SetPlanet(currentPlanet);
    }

    private void CreateVertices()
    {
        int cornerVertices = 8;
        int edgeVertices = (resolution + resolution + resolution - 3) * 4;
        int faceVertices = (
            (resolution - 1) * (resolution - 1) +
            (resolution - 1) * (resolution - 1) +
            (resolution - 1) * (resolution - 1)) * 2;
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        normals = new Vector3[vertices.Length];
        cubeUV = new Color32[vertices.Length];
        int v = 0;
        for (int y = 0; y <= resolution; y++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                SetVertex(v++, x, y, 0);
            }
            for (int z = 1; z <= resolution; z++)
            {
                SetVertex(v++, resolution, y, z);
            }
            for (int x = resolution - 1; x >= 0; x--)
            {
                SetVertex(v++, x, y, resolution);
            }
            for (int z = resolution - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }
        for (int z = 1; z < resolution; z++)
        {
            for (int x = 1; x < resolution; x++)
            {
                SetVertex(v++, x, resolution, z);
            }
        }
        for (int z = 1; z < resolution; z++)
        {
            for (int x = 1; x < resolution; x++)
            {
                SetVertex(v++, x, 0, z);
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.colors32 = cubeUV;
    }

    private void ApplyColor()
    {
        Vector3 planetDirection = currentPlanet.transform.position;
        colors = new Color32[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(planetDirection, mesh.vertices[i]);
            float normalizedDistance = Mathf.Clamp01(Mathf.InverseLerp(-currentBiome.noiseScale, currentBiome.noiseScale, planetSize - distance));

            colors[i] = currentBiome.heightGradient.Evaluate(normalizedDistance);
        }

        mesh.colors32 = colors;
    }

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 v = new Vector3(x, y, z) * 2f / resolution - Vector3.one;
        normals[i] = v.normalized;
        vertices[i] = normals[i] * planetSize;
        cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
    }

    private void CreateTriangles()
    {
        int[] trianglesZ = new int[(resolution * resolution) * 12];
        int[] trianglesX = new int[(resolution * resolution) * 12];
        int[] trianglesY = new int[(resolution * resolution) * 12];
        int ring = (resolution + resolution) * 2;
        int tZ = 0, tX = 0, tY = 0, v = 0;
        for (int y = 0; y < resolution; y++, v++)
        {
            for (int q = 0; q < resolution; q++, v++)
            {
                tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < resolution; q++, v++)
            {
                tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < resolution; q++, v++)
            {
                tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < resolution - 1; q++, v++)
            {
                tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
            }
            tX = SetQuad(trianglesX, tX, v, v - ring + 1, v + ring, v + 1);

        }
        tY = CreateTopFace(trianglesY, tY, ring);
        tY = CreateBottomFace(trianglesY, tY, ring);

        mesh.subMeshCount = 3;
        mesh.SetTriangles(trianglesZ, 0);
        mesh.SetTriangles(trianglesX, 1);
        mesh.SetTriangles(trianglesY, 2);
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int v = ring * resolution;
        for (int x = 0; x < resolution - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);
        int vMin = ring * (resolution + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for (int z = 1; z < resolution - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + resolution - 1);
            for (int x = 1; x < resolution - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid, vMid + 1, vMid + resolution - 1, vMid + resolution);
            }
            t = SetQuad(triangles, t, vMid, vMax, vMid + resolution - 1, vMax + 1);
        }
        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < resolution - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
        return t;
    }

    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = vertices.Length - (resolution - 1) * (resolution - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < resolution - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);
        int vMin = ring - 2;
        vMid -= resolution - 2;
        int vMax = v + 2;
        for (int z = 1; z < resolution - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + resolution - 1, vMin + 1, vMid);
            for (int x = 1; x < resolution - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid + resolution - 1, vMid + resolution, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + resolution - 1, vMax + 1, vMid, vMax);
        }
        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < resolution - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        return t;
    }

    private static int
    SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private void ShadeFlat()
    {
        Vector3[] oldVerts = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = oldVerts[triangles[i]];
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void AddNoise()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * noise.Evaluate(vertices[i]) * currentBiome.noiseScale;
        }

        mesh.vertices = vertices;
    }

    public void UnfreezeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<EnemyControls>().canMove = true;
        }
    }

    private void PlaceMustSpawns()
    {
        StartCoroutine(SpawnObjects(currentBiome.mustSpawnedItems, currentBiome.mustSpawnedItemAmount, null, PlaceEnemies));
    }

    private void PlaceEnemies()
    {
        StartCoroutine(SpawnObjects(currentBiome.enemies, currentBiome.enemyCount, enemies, UpdateResourceCount));
    }

    private void UpdateResourceCount()
    {
        planets[0].SetResourceAmount(placedResources.Count);
        ScoreManager.instance.SetVisualTotalMinerals(placedResources.Count);
        ScoreManager.instance.SetMineralCount(0);
        StartCoroutine(SpawnObjects(currentBiome.vegetation, currentBiome.vegetationAmount, null, EndGeneration, .5f));
    }

    private void EndGeneration()
    {
        onPlanetFinishedGeneration.Invoke();
        spawnPrevention.SetActive(false);
    }

    private void PlaceColliderlessObjects()
    {
        if (currentBiome.noColliderAmount <= 0 || currentBiome.noColliderObjects.Count <= 0) return;
        for (int i = 0; i < currentBiome.noColliderAmount; i++)
        {
            PlanetObject noColliderObject = currentBiome.noColliderObjects[Random.Range(0, currentBiome.noColliderObjects.Count)];
            Vector3 spawnPoint = currentPlanet.transform.TransformPoint(Random.onUnitSphere * (planetSize - 0.2f));
            GameObject spawnedObject = LeanPool.Spawn(noColliderObject.objectPrefab, currentPlanet.transform);
            spawnedObject.transform.position = spawnPoint;
            spawnedObject.transform.rotation = Quaternion.LookRotation(spawnPoint - currentPlanet.transform.position, currentPlanet.transform.right);
            spawnedObject.transform.localScale = Vector3.one * (1f + Random.Range(-noColliderObject.objectScaleMargin, noColliderObject.objectScaleMargin));
            spawnedObject.transform.localRotation *= Quaternion.Euler(90, 0, 0);
        }
    }

    bool IsOccupied(Vector3 checkPosition, float checkRadius)
    {
        return Physics.CheckSphere(checkPosition, checkRadius, ~structureIgnoreLayers);
    }

    bool SpawnOnRandomPosition(GameObject objectToSpawn, float checkRadius = 2f, List<GameObject> outputList = null, float sizeDifference = 0)
    {
        for (int j = 0; j < spawnIterations; j++)
        {
            Vector3 potentialSpawnpoint = currentPlanet.transform.TransformPoint(Random.onUnitSphere * (planetSize - 0.2f));
            if (!IsOccupied(potentialSpawnpoint, checkRadius))
            {
                GameObject spawnedObject = LeanPool.Spawn(objectToSpawn, currentPlanet.transform);
                spawnedObject.transform.position = potentialSpawnpoint;
                spawnedObject.transform.rotation = Quaternion.LookRotation(potentialSpawnpoint - currentPlanet.transform.position, currentPlanet.transform.right);
                spawnedObject.transform.localScale = Vector3.one * (1f + Random.Range(-sizeDifference, sizeDifference));
                spawnedObject.transform.localRotation *= Quaternion.Euler(90, 0, 0);
                spawnedObject.transform.Rotate(Vector3.up * Random.Range(0, 360f), Space.Self);
                if (outputList != null) outputList.Add(spawnedObject);
                return true;
            }
        }
        return false;
    }

    IEnumerator SpawnObjects(PlanetObject planetObject, int spawnAmount = 5, List<GameObject> outputList = null, System.Action finishedAction = null, float finsishedActionDelay = 0f)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            if (!SpawnOnRandomPosition(planetObject.objectPrefab, planetObject.objectRadius, outputList, planetObject.objectScaleMargin))
            {
                Debug.Log("Unable to spawn object");
            }
            yield return new WaitForEndOfFrame();
        }

        if (finishedAction != null)
        {
            yield return new WaitForSeconds(finsishedActionDelay);
            finishedAction();
        }
    }

    IEnumerator SpawnObjects(List<PlanetObject> planetObject, int spawnAmount = 5, List<GameObject> outputList = null, System.Action finishedAction = null, float finsishedActionDelay = 0f)
    {
        if (planetObject.Count > 0)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                PlanetObject objectToSpawn = planetObject[Random.Range(0, planetObject.Count)];
                if (!SpawnOnRandomPosition(objectToSpawn.objectPrefab, objectToSpawn.objectRadius, outputList, objectToSpawn.objectScaleMargin))
                {
                    Debug.Log($"Unable to spawn object '{objectToSpawn.objectPrefab.name}'");
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (finishedAction != null)
        {
            yield return new WaitForSeconds(finsishedActionDelay);
            finishedAction();
        }
    }
}
