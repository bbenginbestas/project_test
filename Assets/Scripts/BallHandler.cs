using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballRigidBody;
    [SerializeField] private Rigidbody2D pivotSpringJoint;
    [SerializeField] private float launchDelay;
    [SerializeField] private float respawnTime;

    private SpringJoint2D currentSpring;
    private Rigidbody2D currentBall;
    private Camera gameCamera;
    private bool isDragged;


    // Start is called before the first frame update
    void Start()
    {
        gameCamera = Camera.main;
        SpawnBall();
        isDragged = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBall != null)
        {
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                isDragged = true;
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                Vector2 worldPosition = gameCamera.ScreenToWorldPoint(touchPosition);
                currentBall.isKinematic = true;
                currentBall.position = worldPosition;
            }
            else
            {
                if (isDragged == true)
                {
                    isDragged = false;
                    currentBall.isKinematic = false;
                    Invoke(nameof(LaunchBall), launchDelay);
                    Invoke(nameof(SpawnBall), respawnTime);
                }
            }
        }

    }
    private void LaunchBall()
    {
        currentSpring.connectedBody = null;
        currentBall = null;
    }

    private void SpawnBall()
    {
        GameObject tempBall = Instantiate(ballRigidBody, pivotSpringJoint.position, Quaternion.identity);
        currentBall = tempBall.GetComponent<Rigidbody2D>();
        currentSpring = pivotSpringJoint.GetComponent<SpringJoint2D>();
        
        currentSpring.connectedBody = currentBall;
    }
}
