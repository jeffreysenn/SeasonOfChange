using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PhysicsMovementComponent : MonoBehaviour {

    public float forceForward = 10;
    public float forceRight = 10;
    public float speedJump = 8;
    public float speedDash = 10;
    public float dashCoolDown = .5f;
    public float speedSlam = 20;

    public float airControlForward = .5f;
    public float airControlRight = .5f;

    public float groundCheckExtraRadius = -.01f;
    public float groundCheckOvershoot = .1f;

    private bool shouldJump = false, shouldDash = false;
    private bool shouldSlam = false;
    private float verticleAxisValue, horizontalAxisValue;
    private Vector3 force = Vector3.zero;

    private MovementState state = MovementState.Jumping;

    private bool haveJumpDashed = false;
    private float dashTimer = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (!IsMovingOnGround()) { state = MovementState.Jumping; }

        switch (state)
        {
            case MovementState.Grounding:
                MoveForward(verticleAxisValue);
                MoveRight(horizontalAxisValue);
                if (shouldJump)
                {
                    Jump();
                    state = MovementState.Jumping;
                }
                if (shouldDash)
                {
                    Dash();
                    state = MovementState.Dashing;
                }
                
                break;
            case MovementState.Jumping:
                MoveForward(airControlForward * verticleAxisValue);
                MoveRight(airControlRight * horizontalAxisValue);
                if (IsMovingOnGround())
                {
                    haveJumpDashed = false;
                    state = MovementState.Grounding;
                }
                if (shouldSlam)
                {
                    Slam();
                    state = MovementState.Slamming;
                }
                if (shouldDash && !haveJumpDashed)
                {
                    Dash();
                    haveJumpDashed = true;
                    state = MovementState.Dashing;
                }
                ClearRequests();
                break;
            case MovementState.Dashing:
                dashTimer += Time.fixedDeltaTime;
                if(dashTimer > dashCoolDown)
                {
                    dashTimer = 0;
                    state = MovementState.Jumping;
                }
                ClearRequests();
                break;
            case MovementState.Slamming:
                shouldSlam = false;
                if(GetComponent<Rigidbody>().velocity.y > Physics.gravity.y) { state = MovementState.Slamming; }
                break;

            default:
                break;
        }

        GetComponent<Rigidbody>().AddForce(force);
    }

    public void RequestMoveForward(float axisValue) { verticleAxisValue = axisValue; }
    public void RequestMoveRight(float axisValue) { horizontalAxisValue = axisValue; }
    public void RequestJump() { shouldJump = true; }
    public void RequestDash() { shouldDash = true; }
    public void RequestSlam() { shouldSlam = true; }
    private void ClearRequests()
    {
        shouldJump = false;
        shouldDash = false;
    }

    private void MoveForward(float axisValue) { force.z = axisValue * forceForward; }
    private void MoveRight(float axisValue) { force.x = axisValue * forceRight; }
    private void Jump() { GetComponent<Rigidbody>().velocity = Vector3.up * speedJump; }
    public void Dash() { GetComponent<Rigidbody>().velocity = force.normalized * speedDash; }
    public void Slam() { GetComponent<Rigidbody>().velocity = new Vector3(0, -speedSlam, 0); }

    public bool IsMovingOnGround()
    {
        Ray ray = new Ray(GetComponent<Renderer>().bounds.center + Vector3.up * (GetCapsuleHalfHeight() - GetGroundCheckRadius()), -Vector3.up);
        return Physics.SphereCast(ray, GetGroundCheckRadius(), 2 * (GetCapsuleHalfHeight() - GetGroundCheckRadius()) + GetGroundCheckOvershoot());
    }
    private float GetCapsuleCylinderHalfHeight() { return (GetComponent<CapsuleCollider>().height / 2 - GetComponent<CapsuleCollider>().radius) * transform.localScale.y; }
    private float GetCapsuleRadius() { return GetComponent<CapsuleCollider>().radius * Mathf.Max(transform.localScale.x, transform.localScale.z); }
    private float GetCapsuleHalfHeight() { return GetComponent<CapsuleCollider>().height / 2 * transform.localScale.y; }
    private float GetGroundCheckRadius() { return (GetComponent<CapsuleCollider>().radius - groundCheckExtraRadius) * Mathf.Max(transform.localScale.x, transform.localScale.z); }
    private float GetGroundCheckOvershoot() { return groundCheckOvershoot * transform.localScale.y; }


    public MovementState GetMovementState() { return state; }
}
