using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //UI texts
    public Text scoreText,
        minutesText,
        secondsText;

    public Text GOScore,
        GOTime;

    public Text Highscore1,
        Highscore2,
        Highscore3,
        Highscore4,
        Highscore5;

    //Objects
    public GameObject foodie;
    public snakeScript snake;
    public GameObject gameOverCanvas;

    //Data
    int score = 0;
    public float foodPoints;
    public float difficulty;
    float scoreMultiplier = 1.0f;
    int[] highscores = new int[6];

    float curTime = 0;
    int secs = 0, mins = 0;
    void timeCountnDisplay()
    {
        secs++;
        if (secs % 60 == 0)
        {
            secs = 0;
            mins++;
            if (mins % 60 == 0) //ttas bn loco carnal
                mins = 0;

            //Display/Update minutes.
            if (mins < 10)
                minutesText.text = "0" + mins.ToString();
            else
                minutesText.text = mins.ToString();
        }
        if (secs < 10)
            secondsText.text = "0" + secs.ToString();
        else
            secondsText.text = secs.ToString();
    }

    //Event Overriders
    void spawnFood()
    {
        foodie = Instantiate(foodie, newRandPos(), Quaternion.identity);
    }
        //Create a new position for foodie such that it doesn't spawn on snake's position.
        Vector3 newRandPos()
        {
            Vector3 newPos = new Vector3(Random.Range(0, 9) + 0.5f, Random.Range(0, 12) + 0.5f,-10);

            if (newPos == snake.head.transform.position)
                return newRandPos();

            for (int i = 0; i < snake.B.Count; i++)
            {
                if (newPos == snake.B[i].transform.position)
                    return newRandPos();
            }
            return newPos;
        }

    void getPointsnDisplay()
    {
        score += Mathf.FloorToInt(foodPoints *difficulty *scoreMultiplier);

        if (score < 1000)
            scoreText.text = "00000" + score.ToString();
        else if (score < 10000)
            scoreText.text = "0000" + score.ToString();
        else if (score < 100000)
            scoreText.text = "000" + score.ToString();
        else if (score < 1000000)
            scoreText.text = "00" + score.ToString();
        else if (score < 10000000)
            scoreText.text = "0" + score.ToString();
        else 
            scoreText.text = score.ToString();
    }

    string score2str(int s)
    {
        if (s < 1000)
            return "00000" + s;
        else if (s < 10000)
            return "0000" + s;
        else if (s < 100000)
            return "000" + s;
        else if (s < 1000000)
            return "00" + s;
        else if (s < 10000000)
            return "0" + s;
        else
            return s.ToString();
    }

    void gameOver(GameObject b)
    {
        Color32 deathColor = new Color32(232, 21, 21, 1);
        snake.head.GetComponent<Renderer>().material.SetColor("_EmissionColor", deathColor);


        GOScore.text = score.ToString();
        if (mins == 0)
            GOTime.text = secs.ToString() + " secs";
        else
        {
            if (secs < 10)
                GOTime.text = mins.ToString() + ":0" + secs.ToString();
            else
                GOTime.text = mins.ToString() + ":" + secs.ToString();
        }

        gameOverCanvas.SetActive(true);

        //la  neta nose k vrgs hice aki
        bool done = false;
        for (int i = 0; i < 5; i++)
        {
            int k =5; 
            if (score >= highscores[i] && !done)
            {
                k = i;
                for (int c = 4; c >= k; c--)
                {
                    //highscores[c + 1] = highscores[c];
                    PlayerPrefs.SetInt("top" + (c + 1), PlayerPrefs.GetInt("top" + c));
                }
                done = true;
            }
           
            //highscores[k] = score;
            PlayerPrefs.SetInt("top" + (k + 1), score);
        }

            Time.timeScale = 0;
    }

    void Start ()
    {
        scoreText.text = "00000000";
        minutesText.text = "00";
        secondsText.text = "00";

        //Load highscores
        for (int i = 0; i < 5; i++)
        {
            string str = "top" + (i + 1);
            highscores[i] = PlayerPrefs.GetInt(str);
        }
        Highscore1.text = score2str(highscores[0]);
        Highscore2.text = score2str(highscores[1]);
        Highscore3.text = score2str(highscores[2]);
        Highscore4.text = score2str(highscores[3]);
        Highscore5.text = score2str(highscores[4]);


        spawnFood();
        snake.head.GetComponent<Collision>().foodTaken += spawnFood;
        snake.head.GetComponent<Collision>().foodTaken += getPointsnDisplay;

        snake.head.GetComponent<Collision>().bodyHit += gameOver;

    }
	
	void Update ()
    {
        if (Time.timeSinceLevelLoad >= curTime + 1.0f)
        {
            timeCountnDisplay();
            curTime += 1.0f;
        }

        if (gameOverCanvas && Input.GetKeyDown(KeyCode.Return))
        {
            score = 0;
            secs = 0; mins = 0;

            SceneManager.LoadScene(0);
        }


    }

}
