using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    //private MovementComponent movementComp;

    //void Start()
    //{
    //    GameObject playerCharacterObj = GameObject.FindGameObjectWithTag("Player");
    //    if (playerCharacterObj == null)
    //    {
    //        Debug.Log("Player object not found");
    //        return;
    //    }

    //    movementComp = playerCharacterObj.GetComponent<MovementComponent>();
    //    if (movementComp == null)
    //    {
    //        Debug.Log("Player movement component not found");
    //        return;
    //    }
    //}

    //void Update()
    //{
    //    movementComp.RequestMoveForward(Input.GetAxis("Vertical"));
    //    movementComp.RequestRotateRight(Input.GetAxis("Horizontal"));
    //    if (Input.GetButton("Jump")) { movementComp.RequestJump(); }

    //}

    private PhysicsMovementComponent physicsMovementComp;

    void Start()
    {
        GameObject playerCharacterObj = GameObject.FindGameObjectWithTag("Player");
        if (playerCharacterObj == null)
        {
            Debug.Log("Player object not found");
            return;
        }
        physicsMovementComp = playerCharacterObj.GetComponent<PhysicsMovementComponent>();
        if (physicsMovementComp == null)
        {
            Debug.Log("Player movement component not found");
            return;
        }
    }

    void Update()
    {
        physicsMovementComp.RequestMoveForward(Input.GetAxis("Vertical"));
        physicsMovementComp.RequestMoveRight(Input.GetAxis("Horizontal"));
        if (Input.GetButton("Jump")) { physicsMovementComp.RequestJump(); }

    }
}

