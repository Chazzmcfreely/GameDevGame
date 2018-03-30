using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopCollision : MonoBehaviour {

    GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.tag == "scoop" && transform.position.y > gameManager.topScoop.transform.position.y) {
            Destroy(GetComponent<Rigidbody2D>());
            transform.SetParent(gameManager.cone.transform);

            gameManager.topScoop = gameObject;
            gameManager.score++;
        }
    }


}
