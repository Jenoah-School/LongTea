using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyControls : MonoBehaviour
{
    enum enemyType { shooter, brawler }

    [SerializeField] enemyType type;

    public bool canShootPlayer;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float fieldOfView = 90;
    [SerializeField] private float detectionDistance = 15;
    [SerializeField] private LayerMask detectionIgnoreLayers;
    [SerializeField] private SimpleAnimation simpleAnimation;
    [SerializeField] private float shootingDistance = 0;

    private Rigidbody rb;    
    private float roamingDelay = 0;
    private float rotateSpeed = 90;
    private float rotationAngle;
    private bool canStartRoaming;
    private bool hasDetectedPlayer;
    private GameObject player;
    private Vector3 moveDirection;
    private Vector3 currentDirection;
    private Vector3 lookDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        rotationAngle = Random.Range(0, 360);

        StartCoroutine(RoamingMode());
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, 2, ~detectionIgnoreLayers) && !hasDetectedPlayer)
        {
            currentDirection = Vector3.zero;

            if (canStartRoaming)
            {
                canStartRoaming = false;
                StopCoroutine(RoamingMode());
                roamingDelay = 0;
                StartCoroutine(RoamingMode());
            }
        }

        moveDirection = currentDirection.normalized * (moveSpeed / 100);
        moveDirection = transform.TransformDirection(moveDirection);
        rb.MovePosition(rb.position + moveDirection);

        PlayerDetection();

        Debug.DrawLine(transform.position, transform.position + lookDirection, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        Debug.DrawLine(transform.position, player.transform.position, Color.white);
    }

    IEnumerator RoamingMode()
    {
        canShootPlayer = false;
        rotateSpeed = 90;
        canShootPlayer = false;
        currentDirection = new Vector3(0, 0, 1);
        if (simpleAnimation != null) simpleAnimation.StartMoving();

        yield return new WaitForSeconds(roamingDelay);

        roamingDelay = Random.Range(2.5f, 5f);     

        lookDirection = Quaternion.AngleAxis(rotationAngle, transform.up) * transform.forward;
        currentDirection = Vector3.zero;
        if (simpleAnimation != null) simpleAnimation.StopMoving();

        if (Physics.Raycast(transform.position, lookDirection, 8, ~detectionIgnoreLayers))
        {
            while (Physics.Raycast(transform.position, lookDirection, 8, ~detectionIgnoreLayers))
            {
                if (rotationAngle > 180)
                {
                    lookDirection = Quaternion.AngleAxis((-1), transform.up) * lookDirection;
                }
                else
                {
                    lookDirection = Quaternion.AngleAxis((1), transform.up) * lookDirection;
                }
                yield return new WaitForEndOfFrame();
            }
            lookDirection = rotationAngle > 180 ? Quaternion.AngleAxis(-15, transform.up) * lookDirection : Quaternion.AngleAxis(15, transform.up) * lookDirection;
        }

        rotationAngle = Random.Range(0, 360);
        StartCoroutine(RotateToTargetVector());
    }

    void PlayerDetection()
    {
        //Check if player is close enough to the enemy
        if(Vector3.Distance(player.transform.position, transform.position) < detectionDistance)
        {
            //Check if player is in field of view
            if(Vector3.Dot(player.transform.position - transform.position, transform.forward) / Vector3.Magnitude(player.transform.position - transform.position) > Mathf.Cos(fieldOfView / 2) || Vector3.Distance(player.transform.position, transform.position) < (detectionDistance / 2))
            {
                if (!Physics.Raycast(transform.position, player.transform.position - transform.position, detectionDistance, ~detectionIgnoreLayers))
                {
                    if (!hasDetectedPlayer)
                    {
                        StopCoroutine(RoamingMode());
                        StopCoroutine(RotateToTargetVector());
                        hasDetectedPlayer = true;
                        rotateSpeed = 180;
                        StartCoroutine(RotateToTargetVector());
                    }
                }
            }
            if (hasDetectedPlayer)
            {
                //Set the look direction of the enemy pointing to the player, on the plane of the forward vector
                if (!Physics.Raycast(transform.position, player.transform.position - transform.position, detectionDistance, ~detectionIgnoreLayers))
                {
                    Vector3 differenceVector = player.transform.position - transform.position;
                    lookDirection = Vector3.ProjectOnPlane(differenceVector.normalized, transform.up);
                    if(type == enemyType.shooter && Vector3.Distance(transform.position, player.transform.position) < shootingDistance)
                    {
                        canShootPlayer = true;
                    }
                    else
                    {
                        canShootPlayer = false;
                        
                    }
                    currentDirection = new Vector3(0, 0, 1);
                    if (simpleAnimation != null) simpleAnimation.StartMoving();
                }
                else
                {
                    if(Vector3.Dot(transform.right, player.transform.position - transform.position) > 0)
                    {
                        rotationAngle = 350;
                    }
                    else
                    {
                        rotationAngle = 10;
                    }
                    hasDetectedPlayer = false;
                    roamingDelay = 0;
                    StopAllCoroutines();
                    StartCoroutine(RoamingMode());
                }              
            }
        }
        else
        {
            if(hasDetectedPlayer)
            {
                hasDetectedPlayer = false;
                lookDirection = transform.forward;
            }          
        }
    }

    IEnumerator RotateToTargetVector()
    {
        while (!ApproximateToVector(transform.forward, lookDirection) || hasDetectedPlayer)
        {
            if (Vector3.Dot(transform.right, lookDirection) > 0)
            {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotateSpeed);
            }
            else
            {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * -rotateSpeed);
            }
            
            yield return new WaitForEndOfFrame();
        }

        StopAllCoroutines();
        canStartRoaming = true;
        StartCoroutine(RoamingMode());   
    }

    bool ApproximateToVector(Vector3 v1, Vector3 v2)
    {
        return Vector3.SqrMagnitude(v1 - v2) < 0.0001f;
    }
}
