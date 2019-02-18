using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIControl : MonoBehaviour
{

    public intScriptableObject gold;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "Gold :" + gold.value;
    }
}
