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

    public int playerIndex = 1;

    private GameObject playerCharacterObj;

    void Start()
    {
        GameObject outObj;
        if(FindPlayerCharacter(out outObj)) { PossessCharacter(ref outObj); }
    }

    void Update()
    {
        //GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveForward(Input.GetAxis("Vertical"));
        //GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveRight(Input.GetAxis("Horizontal"));
        //if (Input.GetButton("Jump")) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestJump(); }
        //if (Input.GetButton("Dash")) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestDash(); }

        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveForward(GamePadManager.GamePad(playerIndex).LeftStick.X);
        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveRight(GamePadManager.GamePad(playerIndex).LeftStick.Y);
        if (GamePadManager.GamePad(playerIndex).A.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestJump(); }
        if (GamePadManager.GamePad(playerIndex).B.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestDash(); }
    }

    public bool FindPlayerCharacter(out GameObject playerCharacterObj)
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(obj.GetComponent<PlayerInfo>().playerIndex == playerIndex)
            {
                playerCharacterObj = obj;
                return true;
            }
        }
        playerCharacterObj = null;
        return false;
    }

    public void PossessCharacter(ref GameObject characterObj) { playerCharacterObj = characterObj; }

    public GameObject GetPossessedCharacter() { return playerCharacterObj; }
}

