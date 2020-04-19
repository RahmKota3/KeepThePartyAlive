using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] Transform playerModel;

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
    }

    void ActivateAppropriateAnimations()
    {
        if (rb.velocity.magnitude != 0)
            animator.ActivateAnimation(AnimationType.Walking);
        else
            animator.ActivateAnimation(AnimationType.Idle);
    }

    void RotateModel()
    {
        if(GetMovementVector() != Vector3.zero)
            playerModel.forward = GetMovementVector();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<PlayerAnimations>();
    }

    private void LateUpdate()
    {
        RotateModel();
        ActivateAppropriateAnimations();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
