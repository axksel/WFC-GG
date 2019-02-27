using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class PlayerManager : MonoBehaviour
{
    public intScriptableObject playerHealth;
    public intScriptableObject playerMAxHealth;
    public intScriptableObject playerGold;
    public SkillListScriptableObject BeginnerLoadoutSkill;
    public SkillListScriptableObject BeginnerLoadoutRanged;
    public SkillListScriptableObject equippedLoadoutSkill;
    public SkillListScriptableObject equippedLoadoutRanged;
    public GameObjectList playerList;
    public float count;
    public GameObject enemy;
    public GameObjectList navmeshObjects;
    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();


    private void Start()
    {

        RestartResources();


    }
    // Update is called once per frame
    void Update()
    {
        if (playerHealth.value <= 0)
        {
            GameOver();

        }
        //count += 7 * Time.deltaTime;
        //if (count > 100)
        //{
        //    Instantiate(enemy, transform.position, Quaternion.identity);
        //    count = 0;
        //}
    }

    public void GameOver()
    {
        SceneManager.LoadScene("hub", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("scene3");
        RestartResources();
        
    }

    public void RestartResources()
    {
        playerHealth.value = playerMAxHealth.value;
        playerGold.value = 100;
        equippedLoadoutRanged.list.Clear();
        equippedLoadoutSkill.list.Clear();
        equippedLoadoutRanged.list.Add(BeginnerLoadoutRanged.list[0]);
        equippedLoadoutSkill.list.Add(BeginnerLoadoutSkill.list[0]);
    }

    public void ChangeScene()
    {

        SceneManager.LoadScene("scene3", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("hub");

        
        //for (int i = 0; i < navmeshObjects.list.Count; i++)
        //{
        //    surfaces.Add(navmeshObjects.list[i].GetComponent<NavMeshSurface>());
        //}
        


        //for (int i = 0; i < surfaces.Count; i++)
        //{
        //    surfaces[i].BuildNavMesh();
        //}
    }
}
