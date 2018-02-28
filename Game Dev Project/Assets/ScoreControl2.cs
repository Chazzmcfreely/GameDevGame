
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreControl2 : MonoBehaviour
{

    public static int liveCount = 3;
    Text live;


    // Use this for initialization
    void Start()
    {
        live = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        live.text = "Player Two's Lives: " + liveCount;
    }
}
