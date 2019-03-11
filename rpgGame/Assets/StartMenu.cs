using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public GameObject credits;
    bool inCredits;
    public GameObject settings;
    bool inSettings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
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

    }
}
