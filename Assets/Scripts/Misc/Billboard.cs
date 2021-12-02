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
            Vector3 relativePos;

            if (inverse)
            {
                relativePos = cam.position - transform.position;
            }
            else
            {
                relativePos = transform.position - cam.position;
            }

            Quaternion rotation = Quaternion.LookRotation(relativePos, cam.up);
            transform.rotation = rotation;
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
