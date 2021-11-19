using UnityEngine;

[CreateAssetMenu(fileName = "Planet Object", menuName = "Planet Generation/Planet Object", order = 100)]
public class PlanetObject : ScriptableObject
{
    public GameObject objectPrefab = null;
    public float objectScaleMargin = 0.2f;
    public float objectRadius = 2f;
}