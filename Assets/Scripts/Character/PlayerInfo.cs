﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public float playerIndex = 1;

    public Color color;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}