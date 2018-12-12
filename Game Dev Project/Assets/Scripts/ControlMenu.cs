using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenu : MonoBehaviour {

	public GameObject controls;
	public bool isEnabled;

	// Use this for initialization
	void Start () {
		controls.SetActive (false);



	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Teleport")) 
		{
			if (isEnabled == false) {
				controls.SetActive (true);
				isEnabled = true;
			}

			else if (isEnabled == true)
			{
				controls.SetActive(false);
				isEnabled = false;
			}
		}
		
	}
}
