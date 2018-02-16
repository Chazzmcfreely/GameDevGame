using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour {

    Player player;
    Rigidbody2D myRigidBody;

	void Start () {
        player = GetComponent<Player>();
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 dirToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized;

            //mouse position - player position = player position to mouse
            //player.currentSpeed = player.dashSpeed;
            myRigidBody.AddForce(dirToMouse * 20, ForceMode2D.Impulse);

        }





	}
}
