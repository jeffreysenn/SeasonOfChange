using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1;

    private GameObject playerCharacterObj;

    void Start()
    {
        GameObject outObj;
        if(FindPlayerCharacter(out outObj)) { PossessCharacter(ref outObj); }
    }

    void Update()
    {

        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveForward(GamePadManager.GamePad(playerIndex).LeftStick.Y);
        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveRight(GamePadManager.GamePad(playerIndex).LeftStick.X);
        if (GamePadManager.GamePad(playerIndex).A.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestJump(); }
        if (GamePadManager.GamePad(playerIndex).B.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestDash(); }
        if (GamePadManager.GamePad(playerIndex).X.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestSlam(); }
        if (GamePadManager.GamePad(playerIndex).Y.Pressed) { GamePadManager.GamePad(playerIndex).SetVibration(100, 100, 0.5f); }
    }

    public bool FindPlayerCharacter(out GameObject playerCharacterObj)
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(obj.GetComponent<CharacterInfo>().playerIndex == playerIndex)
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

