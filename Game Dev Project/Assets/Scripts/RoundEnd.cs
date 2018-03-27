using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEnd : MonoBehaviour
{


    bool Player1Win = false;
    bool Player2Win = false;

    public static bool roundOver = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public static void EndRound()
    {
        Time.timeScale = 0.4f;
        roundOver = true;

    }

}

