using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;

    Vector3 downdirection;

    Vector3 moveDirection;
    int speed = 10;

    public LayerMask planetLayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        downdirection = Vector3.Normalize(GameObject.Find("Planet").transform.position - transform.position);
        rb.AddForce(downdirection * 2);

        if (Physics.Raycast(transform.position, downdirection, out RaycastHit hit, planetLayer))
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed * Time.deltaTime;
        moveDirection = transform.TransformDirection(moveDirection);

        rb.MovePosition(rb.position + moveDirection);
    }
}
