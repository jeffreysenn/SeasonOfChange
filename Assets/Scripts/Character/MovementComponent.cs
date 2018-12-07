using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CharacterController))]
public class MovementComponent : MonoBehaviour
{
    public float gravity = 10;
    public float walkSpeed = 5;
    public float sideWalkSpeed = 5;
    public float jumpHeight = 1;
    public float airControlVertical = 1;
    public float airControlHorizontal = 1;
    public float rotationRate = .1f;


    private Vector3 velocity = Vector3.zero;
    private bool shouldJump;
    private float verticleAxisValue, honrizontalAxisValue;

    private MovementState state = MovementState.Jumping;

    protected void Update()
    {
        GetComponent<CharacterController>().Move(transform.TransformDirection(velocity) * Time.deltaTime);

        if (!GetComponent<CharacterController>().isGrounded) { Fall(); } else { StopFalling(); }

        switch (state)
        {
            case MovementState.Grounding:
                MoveForward(verticleAxisValue);
                RotateRight(honrizontalAxisValue);
                if (shouldJump)
                {
                    Jump();
                    state = MovementState.Jumping;
                }
                break;
            case MovementState.Jumping:
                shouldJump = false;
                MoveForward(airControlVertical * verticleAxisValue);
                RotateRight(airControlHorizontal * honrizontalAxisValue);
                if (GetComponent<CharacterController>().isGrounded) { state = MovementState.Grounding; }
                break;
            default:
                break;
        }
    }

    public void RequestMoveForward(float axisValue) { verticleAxisValue = axisValue; }

    public void RequestRotateRight(float axisValue) { honrizontalAxisValue = axisValue; }

    public void RequestJump() { shouldJump = true; }

    private void MoveForward(float axisValue) { velocity.z = axisValue * walkSpeed; }

    private void RotateRight(float axisValue) { transform.RotateAround(transform.up, axisValue * rotationRate); }

    private void Jump() { velocity.y = Mathf.Sqrt(2 * gravity * jumpHeight); }

    private void Fall() { velocity.y -= gravity * Time.deltaTime; }

    private void StopFalling() { velocity.y = 0; }
}