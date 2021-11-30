using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateOnAxis : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 0.5f;

    public void RotateToCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(-Vector3.forward);

        transform.DOLocalRotateQuaternion(targetRotation, rotationDuration);
    }
    public void RotateAwayFromCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);

        transform.DOLocalRotateQuaternion(targetRotation, rotationDuration);
    }
}
