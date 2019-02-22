using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public intScriptableObject playerHealth;
    TextMeshProUGUI healthBarValue;
    float fullHealth;
    float fillamount;
    Image healthBar;
    public GameObject healthBarImage;
    public GameObject healthBarText;

    void Start()
    {
        fullHealth = playerHealth.value;
        healthBar = healthBarImage.GetComponent<Image>();
        healthBarValue = healthBarText.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        healthBarValue.text = playerHealth.value.ToString();
        if(playerHealth.value / fullHealth != healthBar.fillAmount)
        {
            fillamount = Mathf.Lerp(fillamount, playerHealth.value / fullHealth, 0.05f);
        }
        healthBar.fillAmount = fillamount;
    }
}
