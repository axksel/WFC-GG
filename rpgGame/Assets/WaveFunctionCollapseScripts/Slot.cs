using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slot
{
    public int index =0;
    public bool shouldBeIterated = true;
    public slot[] neighbours = new slot[6];
    public List<GameObject> posibilitySpace = new List<GameObject>();
    public List<GameObject> tmpPosibilitySpace = new List<GameObject>();
    public GameObject instantiatedModule;
    public List<GameObject> neighbour0Pos = new List<GameObject>();
    public List<GameObject> neighbour1Pos = new List<GameObject>();
    public List<GameObject> neighbour2Pos = new List<GameObject>();
    public List<GameObject> neighbour3Pos = new List<GameObject>();
    public List<GameObject> neighbour4Pos = new List<GameObject>();
    public List<GameObject> neighbour5Pos = new List<GameObject>();
    public float noiseWeight;

    public bool IsInstantiated = false;


    public void RemoveZeroWeightModules()
    {

        for (int i = posibilitySpace.Count - 1; i > 0; i--)
        {

            if (posibilitySpace[i].GetComponent<Modulescript>().weight == 0)
            {

                posibilitySpace.RemoveAt(i);

            }
        }

    }

   

    public void Iterate()
    {

        neighbour0Pos.Clear();
        neighbour1Pos.Clear();
        neighbour2Pos.Clear();
        neighbour3Pos.Clear();
        neighbour4Pos.Clear();
        neighbour5Pos.Clear();

        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
            {
                for (int k = 0; k < neighbours[i].posibilitySpace.Count; k++)
                {
                    if (i == 0) neighbour0Pos.AddRange(neighbours[i].posibilitySpace[k].GetComponent<Modulescript>().neighbourSouth);
                    if (i == 1) neighbour1Pos.AddRange(neighbours[i].posibilitySpace[k].GetComponent<Modulescript>().neighbourWest);
                    if (i == 2) neighbour2Pos.AddRange(neighbours[i].posibilitySpace[k].GetComponent<Modulescript>().neighbourNorth);
                    if (i == 3) neighbour3Pos.AddRange(neighbours[i].posibilitySpace[k].GetComponent<Modulescript>().neighbourEast);
                    if (i == 4) neighbour4Pos.AddRange(neighbours[i].posibilitySpace[k].GetComponent<Modulescript>().neighbourUp);
                    if (i == 5) neighbour5Pos.AddRange(neighbours[i].posibilitySpace[k].GetComponent<Modulescript>().neighbourDown);
                }
            }
        }
        
            if (neighbours[0] != null) compareNeighbours(neighbour0Pos);
            if (neighbours[1] != null) compareNeighbours(neighbour1Pos);
            if (neighbours[2] != null) compareNeighbours(neighbour2Pos);
            if (neighbours[3] != null) compareNeighbours(neighbour3Pos);
            if (neighbours[4] != null) compareNeighbours(neighbour4Pos);
            if (neighbours[5] != null) compareNeighbours(neighbour5Pos);

     
    }


    public void SetNeighboursTrue()
    {
        shouldBeIterated = true;
        if (neighbours[0] != null) neighbours[0].shouldBeIterated = true;
        if (neighbours[1] != null) neighbours[1].shouldBeIterated = true;
        if (neighbours[2] != null) neighbours[2].shouldBeIterated = true;
        if (neighbours[3] != null) neighbours[3].shouldBeIterated = true;
        if (neighbours[4] != null) neighbours[4].shouldBeIterated = true;
        if (neighbours[5] != null) neighbours[5].shouldBeIterated = true;

    }

    public void compareNeighbours(List<GameObject> neighbour)
    {
        for (int i = 0; i < posibilitySpace.Count; i++)
        {
            for (int k = 0; k < neighbour.Count; k++)
            {
                if (GameObject.ReferenceEquals(neighbour[k], posibilitySpace[i]))
                {
                    tmpPosibilitySpace.Add(neighbour[k]);
                    break;
                }
            }
        }

        posibilitySpace.Clear();
        posibilitySpace.AddRange(tmpPosibilitySpace);
        tmpPosibilitySpace.Clear();
    }

    public void collapse()
    {
        //Collapse with noiseWeights
        for (int i = 0; i < posibilitySpace.Count; i++)
        {
            if (posibilitySpace[i].GetComponent<Modulescript>().moduleType == Modulescript.ModuleType.Floor)
            {
                posibilitySpace[i].GetComponent<Modulescript>().weight = noiseWeight;
            }
            else
            {
                posibilitySpace[i].GetComponent<Modulescript>().weight = (1 - noiseWeight) / (posibilitySpace.Count - 1);
            }
        }

        //Collapse with weights
        float weightSum = 0;
        int moduleToCollapse = 0;
        for (int i = 0; i < posibilitySpace.Count; i++)
        {
            weightSum += posibilitySpace[i].GetComponent<Modulescript>().weight;
        }

        float random = Random.Range(0, weightSum);

        for (int i = 0; i < posibilitySpace.Count; i++)
        {
            if (random < posibilitySpace[i].GetComponent<Modulescript>().weight)
            {
                moduleToCollapse = i;
                break;
            }
            random -= posibilitySpace[i].GetComponent<Modulescript>().weight;
        }

        GameObject tmpModule = posibilitySpace[moduleToCollapse];
        posibilitySpace.Clear();
        posibilitySpace.Add(tmpModule);

        //Remove from possibilitySpace
        /*int random2 = Random.Range(0, posibilitySpace.Count);

        if (posibilitySpace.Count > 1)
        {
            posibilitySpace.RemoveAt(random2);
        }*/
    }

    public void collapse(int k)
    {       
        GameObject tmpModule = posibilitySpace[k];
        posibilitySpace.Clear();
        posibilitySpace.Add(tmpModule);        
    }

    public bool Contradiction()
    {

        if (posibilitySpace.Count == 0)
        {
            return true;
        }
        return false;
    }

}
