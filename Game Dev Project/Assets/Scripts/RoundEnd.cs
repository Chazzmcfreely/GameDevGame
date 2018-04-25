using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundEnd : MonoBehaviour
{
    public Text redScoreUIText;
    public Text blueScoreUIText;
    public Image winStrobe;
    public Image blueNum0;
    public Image blueNum1;
    public Image blueNum2;
    public Image blueNum3;
    public Image blueNum4;
    public Image blueNum5;
    public Image redNum0;
    public Image redNum1;
    public Image redNum2;
    public Image redNum3;
    public Image redNum4;
    public Image redNum5;
    string currentBlueNum;
    string currentRedNum;




    //static int blueWinCount = 0;
    //static int redWinCount = 0;

    public static bool Player1Win = false;
    public static bool Player2Win = false;

    public static bool roundOver = false;

    private float reloadTimer = 1f;

    private static bool scored = false;

    // Use this for initialization
    void Start()
    {
        Player1Win = false;
        Player2Win = false;
        Time.timeScale = 1f;
        scored = false;
      
        redScoreUIText.text = ScoreManager.redScore.ToString();
        blueScoreUIText.text = ScoreManager.blueScore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if(roundOver){
            reloadTimer -= Time.deltaTime;

            if(reloadTimer <= 0){
                roundOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if(Player1Win){
            winStrobe.color = new Color(1, 0, 0.2f, 0.65f);
            winStrobe.gameObject.SetActive(true);
        }else if(Player2Win){
            winStrobe.color = new Color(0.26f, 0.8f, 1, 0.65f);
            winStrobe.gameObject.SetActive(true);
        }
        else{
            winStrobe.gameObject.SetActive(false);
            winStrobe.color = Color.white;
        }

    }


    public static void EndRound(string playerColor)
    {
        Time.timeScale = 0.4f;
        roundOver = true;

        if(scored == false){
            if (playerColor == "Blue")
            {
                Player2Win = true;
                ScoreManager.blueScore++;
                if(ScoreManager.blueScore >= 5){
                    SceneManager.LoadScene(3);
                }
                Debug.Log(ScoreManager.blueScore);
            }

            if (playerColor == "Red")
            {
                Player1Win = true;
                ScoreManager.redScore++;
                if (ScoreManager.redScore >= 5)
                {
                    SceneManager.LoadScene(2);
                }
                Debug.Log(ScoreManager.redScore);
            }
            scored = true;
        }

    }

}

