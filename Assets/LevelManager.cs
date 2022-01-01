using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btn_StartTheTutorial()
    {
        GameManager.S.startTime = Time.time;
        Debug.Log("tutorial opened");
        SceneManager.LoadScene("Tutorial");
    }

    public void btn_PlayTheGame()
    {
        GameManager.S.startTime = Time.time;
        SceneManager.LoadScene("Lvl1");
    }

    public void btn_RollCredits()
    {
        GameManager.S.startTime = Time.time;
        SceneManager.LoadScene("Credits");
    }
}
