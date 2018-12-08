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
    public float speedSlam = 20;
    public float speedStopDash = 5;

    public float airControlForward = .5f;
    public float airControlRight = .5f;

    public float groundCheckExtraRadius = -.01f;
    public float groundCheckOvershoot = .1f;

    private bool shouldJump = false, shouldDash = false;
    private bool shouldSlam = false;
    private float verticleAxisValue, horizontalAxisValue;
    private Vector3 force = Vector3.zero;

    private MovementState state = MovementState.Jumping;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
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
                shouldJump = false;
                MoveForward(airControlForward * verticleAxisValue);
                MoveRight(airControlRight * horizontalAxisValue);
                if (shouldSlam)
                {
                    Slam();
                    state = MovementState.Slamming;
                }
                if (shouldDash)
                {
                    Dash();
                    state = MovementState.Dashing;
                }
                if (IsMovingOnGround()) { state = MovementState.Grounding; }
                break;
            case MovementState.Dashing:
                shouldDash = false;
                if(GetComponent<Rigidbody>().velocity.magnitude < speedStopDash) { state = MovementState.Jumping; }
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

    private void MoveForward(float axisValue) { force.z = axisValue * forceForward; }
    private void MoveRight(float axisValue) { force.x = axisValue * forceRight; }
    private void Jump() { GetComponent<Rigidbody>().velocity = Vector3.up * speedJump; }
    public void Dash() { GetComponent<Rigidbody>().velocity = force.normalized * speedDash; }
    public void Slam() { GetComponent<Rigidbody>().velocity = new Vector3(0, -speedSlam, 0); }

    private bool IsMovingOnGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * (GetCapsuleHalfHeight() - GetGroundCheckRadius()), -Vector3.up);
        return Physics.SphereCast(ray, GetGroundCheckRadius(), 2 * (GetCapsuleHalfHeight() - GetGroundCheckRadius()) + GetGroundCheckOvershoot());
    }
    private float GetCapsuleCylinderHalfHeight() { return (GetComponent<CapsuleCollider>().height / 2 - GetComponent<CapsuleCollider>().radius) * transform.localScale.y; }
    private float GetCapsuleRadius() { return GetComponent<CapsuleCollider>().radius * Mathf.Max(transform.localScale.x, transform.localScale.z); }
    private float GetCapsuleHalfHeight() { return GetComponent<CapsuleCollider>().height / 2 * transform.localScale.y; }
    private float GetGroundCheckRadius() { return (GetComponent<CapsuleCollider>().radius - groundCheckExtraRadius) * Mathf.Max(transform.localScale.x, transform.localScale.z); }
    private float GetGroundCheckOvershoot() { return groundCheckOvershoot * transform.localScale.y; }

}
