using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public GameObject topScoop;
    public GameObject cone;

    public int score = 0;
    public float gameTime;

    public Text scoreText;
    public Text gameTimeText;

	// Use this for initialization
	void Start () {
        topScoop = cone;
		
	}
	
	// Update is called once per frame
	void Update () {
        gameTime -= Time.deltaTime;

        if (gameTime <= 0) {
            Time.timeScale = 0;
        }

        UpdateUI();
	}

    void UpdateUI () {
        scoreText.text = score.ToString();
        gameTimeText.text = gameTime.ToString("F0");
    }
}
