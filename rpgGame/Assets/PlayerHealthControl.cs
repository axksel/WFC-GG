using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthControl : MonoBehaviour
{
    public intScriptableObject playerHealth;
    public intScriptableObject playerMAxHealth;


    private void Start()
    {
        playerHealth.value = playerMAxHealth.value;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerHealth.value <= 0)
        {
            GameOver();

        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
