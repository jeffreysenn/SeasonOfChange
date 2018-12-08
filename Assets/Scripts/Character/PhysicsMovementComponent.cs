using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInfo))]
public class PhysicsMovementComponent : MonoBehaviour {

    public float forceForward = 10;
    public float forceRight = 10;
    public float speedJump = 8;
    public float speedDash = 10;
    public float dashCoolDown = .5f;
    public float speedSlam = 20;
    public float slamRadius = 30;
    public float slamImpactSpeed = 40;

    public float airControlForward = .5f;
    public float airControlRight = .5f;

    public float groundCheckPercent = 1.1f;

    public LayerMask layerMask;

    private bool shouldJump = false, shouldDash = false;
    private bool shouldSlam = false;
    private float verticleAxisValue, horizontalAxisValue;
    private Vector3 force = Vector3.zero;

    private MovementState state = MovementState.Jumping;

    private bool haveJumpDashed = false;
    private float dashTimer = 0;

    void Start () {
		
	}
	
	void Update () {
        if (!IsMovingOnGround()) { Debug.Log("not on ground"); }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case MovementState.Grounding:
                MoveForward(verticleAxisValue);
                MoveRight(horizontalAxisValue);
                if (!IsMovingOnGround())
                {
                    state = MovementState.Jumping;
                }
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
                ClearRequests();
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
                if (IsMovingOnGround())
                {
                    Collider[] outAffectedPlayers = new Collider[4];
                    int affectedPlayersNum = Physics.OverlapSphereNonAlloc(transform.position, slamRadius, outAffectedPlayers, layerMask);
                    for (int i = 0; i < affectedPlayersNum; i++)
                    {
                        if (outAffectedPlayers[i].gameObject == gameObject) { continue; }
                        if (outAffectedPlayers[i].GetComponent<Rigidbody>() != null)
                        {
                            Vector3 selfToOther = outAffectedPlayers[i].GetComponent<Renderer>().bounds.center - GetLowestPoint();
                            Vector3 velocity = selfToOther.normalized * slamImpactSpeed / Mathf.Sqrt(selfToOther.magnitude);
                            outAffectedPlayers[i].GetComponent<Rigidbody>().velocity = velocity / outAffectedPlayers[i].GetComponent<Rigidbody>().mass;
                        }
                    }
                    state = MovementState.Jumping;
                }
                ClearRequests();
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
        shouldSlam = false;
    }

    private void MoveForward(float axisValue) { force.z = axisValue * forceForward; }
    private void MoveRight(float axisValue) { force.x = axisValue * forceRight; }
    private void Jump() { GetComponent<Rigidbody>().velocity = Vector3.up * speedJump; }
    public void Dash() { GetComponent<Rigidbody>().velocity = force.normalized * speedDash; }

    public void Slam() { GetComponent<Rigidbody>().velocity = -Vector3.up * speedSlam; }

    public bool IsMovingOnGround()
    {
        RaycastHit outHit;
        if (Physics.Raycast(transform.TransformPoint(GetComponent<CapsuleCollider>().center), Vector3.down,out outHit, 10f))
        {
            Vector3 point0 = transform.TransformPoint(GetComponent<CapsuleCollider>().center + Vector3.down * (GetComponent<CapsuleCollider>().height / 2 - GetComponent<CapsuleCollider>().radius));
            Vector3 point1 = transform.TransformPoint(GetComponent<CapsuleCollider>().center + Vector3.up * (GetComponent<CapsuleCollider>().height / 2 - GetComponent<CapsuleCollider>().radius));

            Collider[] cols = Physics.OverlapCapsule(point0, point1, GetComponent<CapsuleCollider>().radius * groundCheckPercent * transform.localScale.x, ~layerMask);
            foreach(Collider col in cols)
            {
                if(col.transform == outHit.transform)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Vector3 GetLowestPoint()
    {
        Vector3 center = GetComponent<Renderer>().bounds.center;
        float radius = GetComponent<Renderer>().bounds.extents.magnitude;
        return center - Vector3.up * radius;
    }

    public MovementState GetMovementState() { return state; }
}
