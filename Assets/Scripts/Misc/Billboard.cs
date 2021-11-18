using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam = null;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        transform.forward = cam.forward;
    }
}
