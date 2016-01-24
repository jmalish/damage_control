using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneScript : MonoBehaviour {

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenGithub()
    {
        Application.OpenURL("https://github.com/jmalish/damage_control");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
