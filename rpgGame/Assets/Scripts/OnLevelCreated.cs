using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OnLevelCreated : MonoBehaviour
{
    public GameObjectList enemyList;
    public GameObjectList playerList;
    public GameObjectList loadScreen;

    public void ActivateEnemies()
    {

        for (int i = 0; i < enemyList.list.Count; i++)
        {
            if (enemyList.list[i] != null)
            {
                enemyList.list[i].SetActive(true);
                enemyList.list[i].GetComponent<NavMeshAgent>().enabled = true;
                enemyList.list[i].GetComponent<enemyScript>().enabled = true;
            }

        }
    }

    public void DeactivateEnemies()
    {
        for (int i = enemyList.list.Count - 1; i > - 1 ; i--)
        {
            if (enemyList.list[i] != null)
            {
                enemyList.list[i].SetActive(false);
            }
            else
            {
                enemyList.list.RemoveAt(i);
            }
        }
    }

    public void ActivatePlayer()
    {
        playerList.list[0].gameObject.transform.position = new Vector3(10, 0.5f, 0);
        playerList.list[0].gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void DeactiveLoadScreen()
    {
        for (int i = 0; i < loadScreen.list[0].transform.childCount; i++)
        {
            loadScreen.list[0].transform.GetChild(i).gameObject.SetActive(false);
        } 
    }
}
