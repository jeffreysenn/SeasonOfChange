using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDeath : MonoBehaviour {
	
	private void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag("DeathTerrain"))
		{
            //Instantiate (explode, transform.position, transform.rotation);
            RoundController.PlayerDeathCallback(GetComponent<CharacterInfo>().playerIndex);
            Destroy(gameObject);
			print("Meow");
		}
	}
}
