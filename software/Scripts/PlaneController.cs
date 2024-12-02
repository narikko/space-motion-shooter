using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour
{

    public float speed;
    public Rigidbody rb;
    public Transform back;
    public Transform forward;

    public float turnSpeed = 50f; // Turning speed
    public float dragWhenNoInput = 2f; // Drag when there is no input
    public float normalDrag = 0f; // Normal drag when input is present

    private PlayerControl pController;
    


    private void Awake()
    {
        pController = new PlayerControl();
        pController.Airplane.Enable();
    }

    void Update()
    {
        // Get the input direction from your controller
        Vector2 inputDirection = pController.Airplane.Movement.ReadValue<Vector2>();
        inputDirection.Normalize();

        // Convert the 2D input to a 3D direction (assuming movement in the XZ plane)
        Vector3 moveDirection = new Vector3(inputDirection.x, inputDirection.y, 0 );

        // Adjust the drag based on input
        if (moveDirection.magnitude > 0)
        {
            rb.drag = normalDrag; // Less drag with input
        }
        else
        {
            rb.drag = normalDrag; // Increase drag when no input
        }

        // Apply movement and rotation
        MoveAndRotate(moveDirection);
    }

    private void MoveAndRotate(Vector3 direction)
    {
        if (rb != null)
        {
            // Apply forward movement in the object's forward direction
            rb.AddForce(transform.forward * speed, ForceMode.Force);
            rb.AddForce(transform.forward*speed*2);

            // Rotate the object based on input
            if (direction.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
    }
}
