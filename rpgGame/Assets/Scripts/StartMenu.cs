using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    public GameObject credits;
    bool inCredits;
    public GameObject settings;
    bool inSettings;
    public GameObject resumeGame;
    bool inMainMenu;

    private void Start()
    {
        if (SceneManager.sceneCount < 2)
        {
            resumeGame.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.5f,0.5f,0.5f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync(0);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
        SceneManager.LoadSceneAsync("SystemControl", LoadSceneMode.Additive);
    }

    public void Settings()
    {
        inSettings = true;
        for (int i = 0; i < settings.transform.childCount; i++)
        {
            settings.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Credits()
    {
        inCredits = true;
        for (int i = 0; i < credits.transform.childCount; i++)
        {
            credits.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        if (inSettings)
        {
            inSettings = false;
            for (int i = 0; i < settings.transform.childCount; i++)
            {
                settings.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (inCredits)
        {
            inCredits = false;
            for (int i = 0; i < credits.transform.childCount; i++)
            {
                credits.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (inMainMenu)
        {
            ResumeGame();
        }

    }
}
