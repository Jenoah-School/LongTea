using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam = null;
    [SerializeField] private bool highQuality = false;
    [SerializeField] private bool inverse = false;

    private Quaternion originalRotation;

    private void Start()
    {
        cam = Camera.main.transform;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (highQuality)
        {
            if (inverse)
            {
                transform.rotation = cam.rotation * Quaternion.Inverse(originalRotation);
            }
            else
            {
                transform.rotation = cam.rotation * originalRotation;
            }
        }
        else
        {
            if (inverse)
            {
                transform.forward = -cam.forward;
            }
            else
            {
                transform.forward = cam.forward;
            }
        }
    }
}
