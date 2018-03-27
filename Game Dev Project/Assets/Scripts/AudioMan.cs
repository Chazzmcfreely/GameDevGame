using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMan : MonoBehaviour {

    public AudioClip DMX_1;
    public AudioClip DMX_2;
    public AudioClip DMX_3;
    public AudioClip DMX_4;
    public AudioClip DMX_5;
    public AudioClip SoundTrack;
    public AudioSource source;

    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
  
        source = GetComponent<AudioSource>();
        source.PlayOneShot(SoundTrack, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

}
