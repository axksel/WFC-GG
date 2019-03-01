using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slot
{
    public int index =0;
    public slot[] neighbours = new slot[6];
    public List<GameObject> posibilitySpace = new List<GameObject>();
    public List<GameObject> tmpPosibilitySpace = new List<GameObject>();

    public List<GameObject> neighbour0Pos = new List<GameObject>();
    public List<GameObject> neighbour1Pos = new List<GameObject>();
    public List<GameObject> neighbour2Pos = new List<GameObject>();
    public List<GameObject> neighbour3Pos = new List<GameObject>();
    public List<GameObject> neighbour4Pos = new List<GameObject>();
    public List<GameObject> neighbour5Pos = new List<GameObject>();

    public bool collapsed = false;
    
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
        posibilitySpace = new List<GameObject>(tmpPosibilitySpace);
        tmpPosibilitySpace.Clear();
    }

    public void collapse()
    {
        int random = Random.Range(0, posibilitySpace.Count);
        GameObject tmpModule = posibilitySpace[random];
        posibilitySpace.Clear();
        posibilitySpace.Add(tmpModule);       
    }

    public void collapse(int k)
    {
        
        GameObject tmpModule = posibilitySpace[k];
        posibilitySpace.Clear();
        posibilitySpace.Add(tmpModule);        
    }

}
