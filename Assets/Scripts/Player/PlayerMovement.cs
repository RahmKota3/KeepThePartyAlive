using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerStats stats;

    Rigidbody rb;
    PlayerAnimations animator;

    Vector3 GetMovementVector()
    {
        Vector3 movementVector = Vector3.zero;

        movementVector = new Vector3(InputManager.Instance.HorizontalAxis, 0, InputManager.Instance.VerticalAxis);

        if (movementVector.magnitude > 1)
            movementVector = movementVector.normalized;

        return movementVector;
    }

    void MovePlayer()
    {
        rb.velocity = GetMovementVector() * stats.MovementSpeed;

        if (rb.velocity.magnitude != 0)
            animator.ActivateAnimation(AnimationType.Walking);
    }

    void RotatePlayer()
    {
        if(GetMovementVector() != Vector3.zero)
            transform.forward = GetMovementVector();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<PlayerAnimations>();
    }

    private void LateUpdate()
    {
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
