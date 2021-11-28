using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleAnimation : MonoBehaviour
{
    [SerializeField] private float maxRotationAngle = 10f;
    [SerializeField] private Vector3 rotationAxis = Vector3.forward;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpHeight = 0.35f;
    [SerializeField] private float seed = 0f;

    [Header("Lay settings")]
    [SerializeField] private float layGroundHeightOffset = 0.5f;
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private float layDownSpeed = 0.5f;
    [SerializeField] private LayerMask groundCheckLayer;

    [Header("Curves")]
    [SerializeField] private AnimationCurve jumpCurve = new AnimationCurve(new Keyframe(-1f, 1f), new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] private AnimationCurve moveCurve = new AnimationCurve(new Keyframe(-1f, -1f), new Keyframe(1f, 1f));

    [Header("Debug")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool moveUp = true;

    private Vector3 startAngle = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;

    private bool canMakeStepSound = true;
    private bool isStopping = false;

    public bool isLyingDown { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        startAngle = transform.localEulerAngles;
        startPosition = transform.localPosition;
        if (seed > 500) seed = Random.Range(0, 500);
    }

    // Update is called once per frame
    void Update()
    {
        RotateWiggle();
    }

    private void RotateWiggle()
    {
        if (!isMoving && !isStopping) return;

        float moveSin = Mathf.Sin(Time.time * moveSpeed + seed);
        Vector3 targetAngle = (Vector3.one * moveCurve.Evaluate(moveSin) * maxRotationAngle);
        targetAngle.Scale(rotationAxis);

        transform.localEulerAngles = startAngle + targetAngle;
        if(moveUp) transform.localPosition = startPosition + (transform.up * jumpCurve.Evaluate(moveSin) * jumpHeight);

        float roundedSin = (float)System.Math.Round(moveSin, 2);

        if (canMakeStepSound)
        {
            if (roundedSin < 0.05f && roundedSin > -0.05f)
            {
                if (isStopping)
                {
                    isMoving = false;
                    isStopping = false;
                }
            }
        }
        else
        {
            if(roundedSin < -0.05f || roundedSin > 0.05f)
            {
                canMakeStepSound = true;
            }
        }
    }

    public void StopMovingImmediate()
    {
        isStopping = false;
        isMoving = false;
    }

    public void LayOnSide()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundCheckLayer))
        {
            transform.DOLocalRotateQuaternion(Quaternion.FromToRotation(Vector3.right, hit.normal), layDownSpeed);
        }
        else
        {
            transform.DOLocalRotate(startAngle + Vector3.forward * 90f, layDownSpeed);
        }
        transform.DOLocalMove(startPosition - Vector3.up * layGroundHeightOffset, layDownSpeed);
        isLyingDown = true;
    }

    public void StopMoving()
    {
        isStopping = true;
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
