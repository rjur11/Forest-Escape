using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager S;
    private AudioSource sounds;
    public AudioClip enemyDeath;
    public AudioClip heroDeath;
    public AudioClip goblinHit;
    
    private void Awake()
    {
        if (S != null)
        {
            Destroy(gameObject);
        }
        else
        {
            S = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        sounds = GetComponent<AudioSource>();
    }


    public void playGoblinHit()
    {
        sounds.PlayOneShot(goblinHit);
    }
    public void PlayEnemyDeath()
    {
        sounds.PlayOneShot(enemyDeath);
    }
    public void PlayPlayerDeath()
    {
        sounds.PlayOneShot(heroDeath);
    }
}
