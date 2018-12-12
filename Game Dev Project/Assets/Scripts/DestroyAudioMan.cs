using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioMan : MonoBehaviour {

	// Use this for initialization
	void Start () {
			Destroy (GameObject.Find("MenuAudioMan"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
