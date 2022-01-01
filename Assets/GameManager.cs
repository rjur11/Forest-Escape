using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState {  menu, getReady, playing, oops, betweenLevels, gameLost, gameWon}

public class GameManager : MonoBehaviour
{

    public static GameManager S;
    public GameState gameState;

    private int LIVES_AT_START = 3;
    public int livesLeft;
    public int score;

    private GameObject stagePrefab;
    private GameObject currStage;

    public GameObject currPlayer;

    private Text scoreTextOverlay;
    private Text livesOverlay;
    private Text timerText;
    private Text messageOverlay;

    public float startTime;
    private float maxNumSeconds = 180.0f;

    public float bossSpeed = 2.5f;

    public Vector3? lastCheckPointPosition = null;

    public void ResetGame()
    {
        gameState = GameState.menu;
        SceneManager.LoadScene("startMenu");
        livesLeft = LIVES_AT_START;
        score = 0;
        RefreshStage();
    }

    public void SetStagePrefab(GameObject sp, Text sto, Text lo, Text tt, Text mo)
    {
        stagePrefab = sp;
        scoreTextOverlay = sto;
        livesOverlay = lo;
        timerText = tt;
        messageOverlay = mo;
        if (messageOverlay != null)
        {
            messageOverlay.enabled = false;
        }
        lastCheckPointPosition = null;
    }

    public void RefreshStage()
    {
        if (currStage != null)
        {
            DestroyImmediate(currStage);
        }
        if (currPlayer != null)
        {
            DestroyImmediate(currPlayer);
        }
        if (stagePrefab != null)
        {
            currStage = Instantiate(stagePrefab);
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 1)
        {
            currPlayer = players[0];
        }
        else if (players.Length > 1)
        {
            throw new System.Exception("More than 1 player object.");
        }
        else
        {
            currPlayer = null;
        }
        bossSpeed = 2.5f;
    }

    public void KillPlayer()
    {
        if (currPlayer.GetComponent<PlayerMvmt>().dead)
        {
            return;
        }
        currPlayer.GetComponent<PlayerMvmt>().dead = true;
        // switch to dying animation
        currPlayer.GetComponent<Animator>().SetTrigger("Dying");
        AudioManager.S.PlayPlayerDeath();

        Destroy(currPlayer, 0.5f);

        livesLeft--;
        if (livesLeft > 0)
        {
            StartCoroutine(OopsState());
        }
        else
        {
            gameState = GameState.gameLost;
        }
    }

    public void GameWon()
    {
        gameState = GameState.gameWon;
        StartCoroutine(LoadCredits());
    }

    public IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(4.0f);

        SceneManager.LoadScene("Credits");
    }

    public IEnumerator GetReadyState()
    {
        gameState = GameState.getReady;
        if (messageOverlay != null)
        {
            messageOverlay.enabled = false;
        }
        yield return new WaitForSeconds(1.5f);
        StartRound();
    }

    public void StartRound()
    {
        gameState = GameState.playing;
    }

    public IEnumerator OopsState()
    {
        gameState = GameState.oops;

        yield return new WaitForSeconds(1.0f);

        RefreshStage();

        if (lastCheckPointPosition != null)
        {
            currPlayer.transform.position = (Vector3)lastCheckPointPosition;
        }

        StartCoroutine(GetReadyState());
    }

    public IEnumerator BeatTheLevel()
    {
        gameState = GameState.betweenLevels;

        yield return new WaitForSeconds(0.5f);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Tutorial":
                SceneManager.LoadScene("Lvl1");
                break;
            case "Lvl1":
                SceneManager.LoadScene("BossBattle");
                break;
            case "BossBattle":
                SceneManager.LoadScene("Credits");
                break;

        }

        StartCoroutine(GetReadyState());
    }

    private void Start()
    {
        livesLeft = LIVES_AT_START;
        RefreshStage();
    }

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
    private void Update()
    {
        if (scoreTextOverlay != null)
        {
            scoreTextOverlay.text = "Score: " + score;
            scoreTextOverlay.enabled = true;
        }
        if (livesOverlay != null)
        {
            livesOverlay.text = "Lives: " + livesLeft;
            livesOverlay.enabled = true;
        }
        if (timerText != null)
        {
            int secondsLeft = (int)Mathf.Ceil(Mathf.Max((maxNumSeconds - Time.time + startTime), 0.0f));
            timerText.text = "Timer: " + (secondsLeft / 60) + ":" + (secondsLeft % 60).ToString("00");
            timerText.enabled = true;
        }
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene("startMenu");
        }
        else if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (Input.GetKeyDown("3"))
        {
            SceneManager.LoadScene("Lvl1");
        }
        else if (Input.GetKeyDown("4"))
        {
            SceneManager.LoadScene("BossBattle");
        }
        else if (Input.GetKeyDown("5"))
        {
            SceneManager.LoadScene("Credits");
        }
        else if (Input.GetKeyDown("r"))
        {
            RefreshStage();
        }
        else if (Input.GetKeyDown("p"))
        {
            ResetGame();
        }

        if (maxNumSeconds - Time.time + startTime <= 0.0f)
        {
            gameState = GameState.gameLost;
        }

        if ((gameState == GameState.gameLost || gameState == GameState.gameWon) && messageOverlay != null)
        {
            string wonOrLost = gameState == GameState.gameLost ? "lost" : "won";
            messageOverlay.text = "You " + wonOrLost + " the game!\nPress \"p\" to return to the main menu.";
            messageOverlay.enabled = true;
        }
    }
}
