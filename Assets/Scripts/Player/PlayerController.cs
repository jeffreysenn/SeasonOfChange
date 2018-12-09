using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1;
    public int communistIndex = 5;
    public float readyTime = 1;
    public Material material;
    public bool isMonster = false;
    public GameObject initialCharacterObj;
    public GameObject monster;
    public Vector3 monsterSpawnOffset = Vector3.up * 5;


    private GameObject playerCharacterObj;
    private SeasonController seasonController;
    private float readyTimer = 0;
    private bool haveReportedReady = false;


    private void OnEnable()
    {
        SeasonController.OnSeasonChange += ChangeCharacter;
    }

    private void OnDisable()
    {
        SeasonController.OnSeasonChange -= ChangeCharacter;
    }


    void Awake()
    {
        ResetCharacter();
        return;
        
    }

    void Update()
    {
        if(GetPossessedCharacter() == null) { return; }

        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveForward(GamePadManager.GamePad(playerIndex).LeftStick.Y);
        GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestMoveRight(GamePadManager.GamePad(playerIndex).LeftStick.X);
        if (GamePadManager.GamePad(playerIndex).A.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestJump(); }
        if (GamePadManager.GamePad(playerIndex).B.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestDash(); }
        if (isMonster && GamePadManager.GamePad(playerIndex).X.Pressed) { GetPossessedCharacter().GetComponent<PhysicsMovementComponent>().RequestSlam(); }


        if (GamePadManager.GamePad(playerIndex).X.Held) { readyTimer += Time.deltaTime; }
        if (GamePadManager.GamePad(playerIndex).X.Released) { readyTimer = 0; }
        if(readyTimer > readyTime && !haveReportedReady)
        {
            GameObject.FindGameObjectWithTag("SeasonController").GetComponent<SeasonController>().ReportReady();
            readyTimer = 0;
            haveReportedReady = true;

        }
        if(GamePadManager.GamePad(playerIndex).Y.Pressed) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

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
        characterToSet.GetComponent<MeshRenderer>().material = material;
        Destroy(GetPossessedCharacter());
        PossessCharacter(ref characterToSet);
    }

    private void ChangeCharacter()
    {
        if((GameObject.FindGameObjectWithTag("SeasonController").GetComponent<SeasonController>().GetCurrentSeason()) == SEASON.COMMUNISM)
        {
            if (isMonster) { return; }
            TurnIntoCharacter(ref monster, monsterSpawnOffset);
            isMonster = true;
        }
        else if((GameObject.FindGameObjectWithTag("SeasonController").GetComponent<SeasonController>().GetCurrentSeason()) == (SEASON) (playerIndex-1))
        {
            if (isMonster) { return; }
            TurnIntoCharacter(ref monster, monsterSpawnOffset);
            isMonster = true;
        }
        else if(isMonster)
        {
            TurnIntoCharacter(ref initialCharacterObj, Vector3.zero);
            isMonster = false;
        }
    }

    public void ResetCharacter()
    {
        if (initialCharacterObj == null) { return; }
        GameObject characterToSet = GameObject.Instantiate(initialCharacterObj, transform.position, Quaternion.identity);
        characterToSet.GetComponent<CharacterInfo>().playerIndex = playerIndex;
        characterToSet.GetComponent<MeshRenderer>().material = material;
        PossessCharacter(ref characterToSet);
    }
}

