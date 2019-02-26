using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldscript : MonoBehaviour,IsInteracable
{
    public int amount =0;
    public intScriptableObject playerGold;
    float scaleRate = 1f;
    Vector3 scale;
    Vector3 endScale;



    private void Start()
    {
        if (amount == 0)
        {
            amount = Random.Range(30,60);
        }
        endScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }

    private void Update()
    {
       scale.x = Mathf.Lerp(scale.x, endScale.x, this.scaleRate * Time.deltaTime);
        scale.y = scale.x;
        scale.z = scale.x;
        transform.localScale = scale;

    }

    public string ReturnName()
    {

        return "Pick up " + amount + "Gold";
    }
    public void Interact()
    {
        transform.position = new Vector3(0, 0, -1000);
        playerGold.value += amount;
        Destroy(gameObject, 0.05f);

    }
}
