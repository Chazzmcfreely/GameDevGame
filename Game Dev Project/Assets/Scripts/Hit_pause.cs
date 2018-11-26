using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_pause : MonoBehaviour
{

    public float duration = .5f;
    bool isFrozen = false;
    public bool doHitPause = false;

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        if (pendingFreezeDuration < 0 && !isFrozen)
        {
            StartCoroutine(HitPauseNow());

        }
        if (doHitPause == true)
        {
            HitPauseNow();
        }
    }
    float pendingFreezeDuration = 0f;
    public void Freeze()
    {
        pendingFreezeDuration = duration;

    }

    IEnumerator HitPauseNow(){
        isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        pendingFreezeDuration = 0;
        isFrozen = false;
        doHitPause = false;
    }
}
