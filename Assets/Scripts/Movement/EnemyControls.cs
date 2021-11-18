using System.Collections;
using UnityEngine;

public class EnemyControls : MonoBehaviour
{
    private Rigidbody rb;

    float moveSpeed = 0.075f;

    Vector3 moveDirection;
    Vector3 currentDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(RoamingBehaviour());
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = currentDirection.normalized * moveSpeed;
        moveDirection = transform.TransformDirection(moveDirection);
        rb.MovePosition(rb.position + moveDirection);
    }

    IEnumerator RoamingBehaviour()
    {
        currentDirection = new Vector3(0, 0, 1);
        yield return new WaitForSeconds(Random.Range(1, 4));
        currentDirection = new Vector3(0, 0, 0);
        StartCoroutine(RotateDegrees());      
    }

    IEnumerator RotateDegrees()
    {
        float rotationSpeed = 100;
        float elapsedTime = 0f;
        float totalTime = Random.Range(0.2f, 2f);

        int randomValue = Random.Range(0, 2);

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(totalTime + " | " + elapsedTime);
       
            if (randomValue == 1)
            {
                transform.localEulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
            }
            else 
            {
                transform.localEulerAngles -= Vector3.up * rotationSpeed * Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }     
        StartCoroutine(RoamingBehaviour());
    }
}
