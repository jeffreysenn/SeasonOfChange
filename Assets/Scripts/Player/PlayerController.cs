using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1;
    public Color color;
    public bool isMonster = false;
    public GameObject initialCharacterObj;
    public GameObject monster;
    public Vector3 monsterSpawnOffset = Vector3.up * 5;


    private GameObject playerCharacterObj;


    void Start()
    {
        if(initialCharacterObj == null) { return; }
        GameObject characterToSet = GameObject.Instantiate(initialCharacterObj, transform.position, Quaternion.identity);
        characterToSet.GetComponent<CharacterInfo>().playerIndex = playerIndex;
        characterToSet.GetComponent<CharacterInfo>().color = color;
        PossessCharacter(ref characterToSet);
    }

    void Update()
    {
        if(GetPossessedCharacter() == null) { return; }

        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveForward(GamePadManager.GamePad(playerIndex).LeftStick.Y);
        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveRight(GamePadManager.GamePad(playerIndex).LeftStick.X);
        if (GamePadManager.GamePad(playerIndex).A.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestJump(); }
        if (GamePadManager.GamePad(playerIndex).B.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestDash(); }
        if (isMonster && GamePadManager.GamePad(playerIndex).X.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestSlam(); }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    public void TurnIntoCharacter(ref GameObject character, Vector3 spawnPositionOffset)
    {
        if(character == null) { return; }
        GameObject characterToSet = GameObject.Instantiate(character, GetPossessedCharacter().transform.position + spawnPositionOffset, Quaternion.identity);
        characterToSet.GetComponent<CharacterInfo>().ragePercent = GetPossessedCharacter().GetComponent<CharacterInfo>().ragePercent;
        characterToSet.GetComponent<CharacterInfo>().playerIndex = playerIndex;
        Destroy(GetPossessedCharacter());
        PossessCharacter(ref characterToSet);
    }

}

