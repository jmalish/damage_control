using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public static int score;
    public GameObject tutorialField;
    Text scoreText;
    bool tutorial = true;


    void Awake()
    {
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
    }

    IEnumerator FadeTutorialField()
    {
        for (float alphaValue = 1; alphaValue > -.05f; alphaValue -= .05f)
        {
            tutorialField.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaValue);
            yield return new WaitForSeconds(.125f);
        }
    }
}
