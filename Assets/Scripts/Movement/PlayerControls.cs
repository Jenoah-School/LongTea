using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick rotationJoystick;
    [SerializeField] private Transform model;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale == 0) {
            return;
        }
        Vector3 moveDirection;
        if (movementJoystick == null || (movementJoystick != null && movementJoystick.Direction.SqrMagnitude() <= 0))
        {
            moveDirection = new Vector3(Input.GetAxisRaw("HorizontalMove"), 0, Input.GetAxisRaw("VerticalMove")).normalized * speed;
        }
        else
        {
            moveDirection = new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical);
            if (moveDirection.sqrMagnitude > 1f) moveDirection.Normalize();
            moveDirection *= speed;
        }
        moveDirection = transform.TransformDirection(moveDirection);
        rb.MovePosition(rb.position + moveDirection);

        if (rotationJoystick == null || (rotationJoystick != null && rotationJoystick.Direction.SqrMagnitude() <= 0))
        {
            transform.Rotate(0, Input.GetAxisRaw("HorizontalRotate"), 0);
        }
        else
        {
            Vector3 lookDirection = new Vector3(rotationJoystick.Horizontal, 0f, rotationJoystick.Vertical).normalized;
            if (lookDirection != Vector3.zero)
            {
                Vector3 lookRotation = Quaternion.LookRotation(lookDirection).eulerAngles;
                model.localRotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
}
