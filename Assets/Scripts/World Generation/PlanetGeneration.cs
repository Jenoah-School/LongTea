using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlanetGeneration : MonoBehaviour
{
    [SerializeField] private int resolution = 5;
    [SerializeField] public float planetSize = 1f;

    [Header("Structures")]
    [SerializeField] private List<GameObject> trees = new List<GameObject>();
    [SerializeField] private float treeAmount = 7;
    [SerializeField] private float treeScaleMargin = 0.2f;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color32[] cubeUV;

    private void Start()
	{
		Generate();
	}

	private void Generate()
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Planet #" + Random.Range(1000, 9999);
		CreateVertices();
		CreateTriangles();
        ShadeFlat();
        CreateColliders();

        Vegetate();
        PlanetManager.instance.SetPlanet(gameObject);

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

    void ShadeFlat()
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

    void Vegetate()
    {
        for (int i = 0; i < treeAmount; i++)
        {
            GameObject spawnedTree = LeanPool.Spawn(trees[0], transform);
            spawnedTree.transform.localScale = Vector3.one * Random.Range(1f - treeScaleMargin, 1f + treeScaleMargin);
            spawnedTree.transform.position = Random.onUnitSphere * (planetSize - 0.2f);
            spawnedTree.transform.rotation = Quaternion.LookRotation(spawnedTree.transform.position - transform.position, transform.right);
            spawnedTree.transform.localRotation *= Quaternion.Euler(90, 0, 0);
        }
    }



    private void CreateColliders()
    {
        gameObject.AddComponent<SphereCollider>();
    }
}
