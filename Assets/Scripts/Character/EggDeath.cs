using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDeath : MonoBehaviour {

	void EggCollision(Collision col)
	{
		if (col.gameObject.name == "DeathTerrain")
		{
			Destroy(col.gameObject);
		}
	}
}
