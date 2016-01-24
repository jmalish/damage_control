using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public static int score;
    public GameObject tutorialField;
    public Text finalScoreText, gameOverText;
    Text scoreText;
    bool tutorial = true;
    bool fading = false;
    public static bool gameOver = false;

    void Awake()
    {
        //finalScoreText.color = new Color(0, 0, 0, 0);
        scoreText = GetComponent<Text>();
        score = 0;
    }

    void Update()
    {
        scoreText.text = "Score: " + score;

        if (tutorial && score > 4)
        {
            tutorial = false;
            StartCoroutine("FadeTutorialField");
        }

        if (PlayerScript.gameOver && !fading)
        {
            StartCoroutine(FadeFinalScoreUp());
        }

        if (gameOver && !PlayerScript.gameOver)
        {
            score = 0;
            StartCoroutine(FadeFinalScoreUp());
        }
    }

    IEnumerator FadeTutorialField()
    {
        for (float alphaValue = 1; alphaValue > -.05f; alphaValue -= .05f)
        {
            tutorialField.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaValue);
            yield return new WaitForSeconds(.125f);
        }
    }

    IEnumerator FadeFinalScoreUp()
    {
        PlayerScript.gameOver = true;
        fading = true;

        finalScoreText.text = "Game Over\nScore: " + score;

        yield return new WaitForSeconds(.5f);

        for (float alphaValue = 0; alphaValue < 1; alphaValue += .03f)
        {
            //gameOverText.color = new Color(0, 0, 0, alphaValue);
            finalScoreText.color = new Color(0, 0, 0, alphaValue);
            yield return new WaitForSeconds(.02f);
        }
    }
}
