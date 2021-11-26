using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class DestroyInRadius : MonoBehaviour
{
    [SerializeField] private float destroyRadius = 10f;
    [SerializeField] private List<string> tagsOfObjectToDestroy = new List<string>();

    public void DestroyObjects()
    {
        Collider[] checkObjects = Physics.OverlapSphere(transform.position, destroyRadius);
        for (int i = 0; i < checkObjects.Length; i++)
        {
            for (int k = 0; k < tagsOfObjectToDestroy.Count; k++)
            {
                if (checkObjects[i].CompareTag(tagsOfObjectToDestroy[k])) LeanPool.Despawn(checkObjects[i]);
                continue;
            }
        }
    }
}
