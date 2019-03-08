using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OnLevelCreated : MonoBehaviour
{
    public GameObjectList enemyList;
    public GameObjectList playerList;

    public void ActivateEnemies()
    {
        foreach (var item in enemyList.list)
        {
            item.SetActive(true);
            item.GetComponent<NavMeshAgent>().enabled = true;
            item.GetComponent<enemyScript>().enabled = true;
        }
    }

    public void DeactivateEnemies()
    {
        foreach (var item in enemyList.list)
        {
            item.SetActive(false);
        }
    }


    public void ActivatePlayer()
    {
        playerList.list[0].gameObject.transform.position = new Vector3(10, 0.5f, 0);
        playerList.list[0].gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}
