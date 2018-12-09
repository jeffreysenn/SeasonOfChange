using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDeath : MonoBehaviour {

    public AudioClip deathSound;
	
	private void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag("DeathTerrain"))
		{
            //Instantiate (explode, transform.position, transform.rotation);
            RoundController.PlayerDeathCallback(GetComponent<CharacterInfo>().playerIndex);
            SoundManager.PlaySFXRandomized(deathSound);
            Destroy(gameObject);
			print("Meow");
		}
	}
}
