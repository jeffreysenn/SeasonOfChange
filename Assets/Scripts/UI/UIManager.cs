using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {


    public GameObject[] playerControllers;
    public Text[] RageText;
    public Text[] scoreText;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < playerControllers.Length; i++)
        {
            RageText[i].text = (Mathf.RoundToInt(playerControllers[i].GetComponent<PlayerController>().GetPossessedCharacter().GetComponent<CharacterInfo>().ragePercent)).ToString() + " %";
            scoreText[i+1].text = "Score: " + RoundController.GetPlayerInfo(i).Score.ToString();
        }
    }


}
