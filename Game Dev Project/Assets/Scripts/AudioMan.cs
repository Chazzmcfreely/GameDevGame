using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMan : MonoBehaviour {

   
    public AudioClip SoundTrack;
    public AudioSource source;
    int sceneID;

    SpriteRenderer spriteRenderer;
    
    GameObject[] musicObject;

    // Use this for initialization
    void Start () {
        musicObject = GameObject.FindGameObjectsWithTag ("GameMusic");
        if (musicObject.Length == 1 ) {
            GetComponent<AudioSource>().Play ();
        } else {
            for(int i = 1; i < musicObject.Length; i++){
                    Destroy(musicObject[0]);
            }
 
        }
             
 
    }

    void Update()
    {
        sceneID = SceneManager.GetActiveScene().buildIndex;    
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
 

}
