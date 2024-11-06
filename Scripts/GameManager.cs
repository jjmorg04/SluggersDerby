using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    // create instance for scripts to call functions
    public static GameManager instance;

    // text displays for homeruns, scores, pitches left, highscore, and current score
    public TextMeshProUGUI homerunText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pitchCounterText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI roundScoreText;
    
    // start screen and game over screen references
    public GameObject startScreen;
    public GameObject gameOverScreen;

    // int values to display to player
    private int homerunCount;
    private int score;
    private int currScore;
    private int highScore;
    public int totalPitches = 10;


    // boolean value to determine if game has started
    private bool gameStarted = false;

    // public game started value
    public bool GameStarted {
        get { 
            return gameStarted;
        }
    }

    // upon awake, 
    void Awake() {
        if (instance == null) {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        LoadHighScore();
        UpdateHomerunText();
        UpdateScoreText();
        UpdatePitchCounterText();
    }

    // when unity game starts, show start screen
    void Start() {
        ShowStartScreen();
    }

    // display start screen
    public void ShowStartScreen() {
        startScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        highScoreText.text = "High Score: " + highScore.ToString();
        
        // reset game values for fresh game
        ResetGameValues();
        
        // game is not playing anymore, so change boolean to false
        gameStarted = false;
    }

    // display game over screen
    public void ShowGameOverScreen() {
        gameOverScreen.SetActive(true);
        startScreen.SetActive(false);
        roundScoreText.text = "Score: " + score.ToString();
        
        // if current game score is higher than highscore, set new highscore
        if (score > highScore) {
            highScore = score;
            SaveHighScore();
        }
    }

    public void StartGame() {
        
        // when game is started, set boolean to true
        // set start screen to inactive
        if (!gameStarted) {
            gameStarted = true;
            startScreen.SetActive(false);
            ResetGameValues();

            // call Glove script's start round protocol
            FindObjectOfType<Glove>().StartRoundProtocol();
        }
    }   


    // function to increment homerun count and update to UI
    public void IncrementHomerunCount() {
        homerunCount++;
        UpdateHomerunText();
    }

    // add acquired pts to overall score, and currScore
    // run check for bonus pitch
    public void AddScore(int points) {
        score += points;
        currScore += points;
        UpdateScoreText();
        CheckForExtraPitch();
    }

    // as long as pitches left is more than 0, decrement 1, and update UI
    public void UsePitch() {
        if (totalPitches > 0) {
            totalPitches -= 1;
            UpdatePitchCounterText();
        }
    }

    // every 1500 pts, user receives another pitch
    private void CheckForExtraPitch() {
        if (currScore > 1500) {
            totalPitches++;
            currScore -= 1500;
            UpdatePitchCounterText();
        }
    }

    // set homerun text to new count
    public void UpdateHomerunText() {
        if (homerunText != null) {
            homerunText.text = "Homeruns: " + homerunCount.ToString();
        }
    }

    // set score text to new score
    public void UpdateScoreText() {
        if (scoreText != null) {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // set pitch count text to new count
    public void UpdatePitchCounterText() {
        if (pitchCounterText != null) {
            pitchCounterText.text = "" + totalPitches.ToString();
        }
    }

    // save highscore
    private void SaveHighScore() {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    // when a game is launched, load in high score
    private void LoadHighScore() {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // at the end of a game, reset all of the values to default
    // update UI accordingly
    private void ResetGameValues() {
        homerunCount = 0;
        score = 0;
        currScore = 0;
        totalPitches = 10;
        UpdateHomerunText();
        UpdateScoreText();
        UpdatePitchCounterText();
    }

    // when play again is pressed from game over screen
    // display start screen again and reset values
    public void PlayAgain() {
        ShowStartScreen();
        ResetGameValues();
        gameStarted = false;
    }

}
