﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDeath : MonoBehaviour {
	
	private void OnCollisonEnter (Collider col)
	{
		if (col.CompareTag("DeathTerrain"))
		{
			//Instantiate (explode, transform.position, transform.rotation);
			//Destroy
			print("Meow");
		}
	}
}
