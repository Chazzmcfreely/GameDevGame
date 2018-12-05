using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_pause : MonoBehaviour
{
    [Range(0f,1.5f)]
    public float duration = .1f;
    bool isFrozen = false;
    Player player;
  //  public bool doHitPause = false;
    float pendingFreezeDuration = 0f;

    private void Start()
    {
        player = GetComponent<Player>();
      Vector3 playerV = player.velocity;

    }
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        if (pendingFreezeDuration > 0 && !isFrozen)
        {
            StartCoroutine(HitPauseNow());

        }
       /* if (doHitPause == true)
        {
            StartCoroutine(HitPauseNow());
        }
        */
    }

    public void Freeze()
    {
        pendingFreezeDuration = duration;

    }

    IEnumerator HitPauseNow(){
        isFrozen = true; 
        //var pos_x = transform.position.x;
       // var originalVelocity = Player.velocity;
        //veloctiy.velocity 
      //  Player.velocity = new Vector3(0, 0, 0);
        var original = Time.timeScale;
        Time.timeScale = 0f;
        Debug.Log(isFrozen);
        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
       // Player.velocity = originalVelocity;
        pendingFreezeDuration = 0;
        isFrozen = false;
        //doHitPause = false;
    }
}
