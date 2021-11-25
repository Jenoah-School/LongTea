using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreelookCamera : MonoBehaviour
{
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private bool scrollToIncreaseSpeed = true;
    [SerializeField] private float scrollSpeedMultiplier = 0.6f;

    private float mouseY = 0f;
    private float mouseX = 0f;

    // Update is called once per frame
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;

        mouseY = Mathf.Clamp(mouseY, - 89.9f, 89.9f);

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        float horizontal = Input.GetAxis("HorizontalMove");
        float vertical = Input.GetAxis("VerticalMove");

        if (scrollToIncreaseSpeed)
        {
            movespeed += Input.mouseScrollDelta.y * scrollSpeedMultiplier;
            movespeed = Mathf.Clamp(movespeed, 0f, 50f);
        }

        Vector3 moveDir = new Vector3(horizontal, 0, vertical) * Time.deltaTime * movespeed;
        moveDir = transform.TransformDirection(moveDir);
        transform.position += moveDir;
    }
}
