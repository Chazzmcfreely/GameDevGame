using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopFlavor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
