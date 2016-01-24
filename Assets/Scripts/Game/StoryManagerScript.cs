using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoryManagerScript : MonoBehaviour {

    public static int storyStage = 0; // 0 means no dialogue on screen, 1 is tutorial, etc
    public TextAsset dFile;
    public Text dTextBox;
    public GameObject dPanel;
    bool dialogueInProgress, stopPlayerMovement;

    public string[] dLines;

    void Start()
    {
        dPanel.SetActive(false);
        if (GetLines() == 0)
        {
            Debug.Log("Dialogue lines failed to load.");
        }

        dialogueInProgress = false;
    }
    
    void Update()
    {
        if (dialogueInProgress || storyStage == 0)
        {
            return;
        }

        if (storyStage > 0)
        {
            printDialogue(storyStage);
        }
    }

    int GetLines()
    {
        if (dFile != null)
        {
            dLines = dFile.text.Split('\n');
        }

        return dLines.Length;
    }

    void printDialogue(int storyStage)
    {
        if (storyStage == 1) // tutorial
        {
            StartCoroutine(Dialogue(1, 4));
        }
        else if (storyStage == 2) // took too much damage in tutorial
        {
            StartCoroutine(Dialogue(7, 8));
            ScoreManager.gameOver = true;
        }
        else if (storyStage == 3) // attempting to leave tutorial area
        {
            StartCoroutine(Dialogue(11, 12));
        }
        else if (storyStage == 4) // continued to leave tutorial area, player will self destruct
        {
            StartCoroutine(Dialogue(15, 17));
            StartCoroutine(PlayerSelfDestructed());
        }
        else if (storyStage == 5) // tutorial complete
        {
            StartCoroutine(Dialogue(20, 23));
            SpawnManagerScript.canSpawn = true;
        }
        else if (storyStage == 6) // explaining weapons
        {
            StartCoroutine(Dialogue(26, 27));
        }
    }


    IEnumerator Dialogue(int startLine, int endLine)
    {
        PlayerScript.canShoot = false;
        dPanel.SetActive(true);
        dialogueInProgress = true;

        for (int line = startLine; line < endLine + 1; line++)
        {
            dTextBox.text = dLines[line];

            yield return StartCoroutine(WaitForKeyPress("Fire1"));
            yield return new WaitForSeconds(.00000001f);
        }

        dPanel.SetActive(false);

        dialogueInProgress = false;
        PlayerScript.canShoot = true;

        storyStage = 0;
    }

    IEnumerator WaitForKeyPress(string button)
    {
        Time.timeScale = 0;
        while (!Input.GetButtonDown(button))
        {
            yield return null;
        }
        Time.timeScale = 1;
    }

    IEnumerator PlayerSelfDestructed()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }
}
