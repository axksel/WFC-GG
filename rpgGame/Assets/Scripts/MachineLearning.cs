using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLearning : MonoBehaviour
{

    int gridX = 4;
    int gridY = 4;
    int gridZ = 4;

    int xRandom;
    int yRandom;
    int zRandom;

    public List<int> randomPooltmp = new List<int>();
    public List<int> randomPool = new List<int>();
    public ScriptableObjectList moduleSO;
    public List<GameObject> modules = new List<GameObject>();

    public slot[,,] grid;
    public slot[,,] savedMiniGrid;

     GridManager gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GetComponent<GridManager>();
        gridX = gM.gridX;
        gridY = gM.gridY;
        gridZ = gM.gridZ;

        modules.AddRange(moduleSO.list);

        savedMiniGrid = new slot[2, 1, 2];

        for (int i = 0; i < 2; i++)
        {
            for (int k = 0; k < 1; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    //savedMiniGrid[i, k, j] = new slot();
                }
            }
        }

    }



    public void UnCollapse(int sizeX, int sizeY, int sizeZ)
    {
        if (randomPool.Count != 0)
        {
            int indexRandom = Random.Range(0, randomPool.Count);
            xRandom = Mathf.Clamp(randomPool[indexRandom] % gridZ, 0, gridX - 2);
            yRandom = 0;
            zRandom = Mathf.Clamp(randomPool[indexRandom] / gridX, 0, gridZ - 2);
            randomPool.RemoveAt(indexRandom);
        }
        else
        {
            randomPool.AddRange(randomPooltmp);
        }

        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                for (int j = 0; j < sizeZ; j++)
                {

                    savedMiniGrid[i, k, j].posibilitySpace.Clear();
                    savedMiniGrid[i, k, j].posibilitySpace.Add(grid[xRandom + i, yRandom + k, zRandom + j].posibilitySpace[0]);
                    grid[xRandom + i, yRandom + k, zRandom + j].posibilitySpace.Clear();
                    grid[xRandom + i, yRandom + k, zRandom + j].posibilitySpace.AddRange(modules);
                    Destroy(grid[xRandom + i, yRandom + k, zRandom + j].instantiatedModule);
                    grid[xRandom + i, yRandom + k, zRandom + j].IsInstantiated = false;

                }
            }
        }

    }

    public void UnCollapseWithPosition(int sizeX, int sizeY, int sizeZ,int posX,int posY,int posZ)
    {
        if (randomPool.Count != 0)
        {
            int indexRandom = Random.Range(0, randomPool.Count);
            xRandom = Mathf.Clamp(randomPool[indexRandom] % gridZ, 0, gridX - 2);
            yRandom = 0;
            zRandom = Mathf.Clamp(randomPool[indexRandom] / gridX, 0, gridZ - 2);
            randomPool.RemoveAt(indexRandom);
        }
        else
        {
            randomPool.AddRange(randomPooltmp);
        }

        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                for (int j = 0; j < sizeZ; j++)
                {
                    if(posX - (int)(sizeX / 2) + i<=0 || posZ - (int)(sizeZ / 2) + j<=0|| posX - (int)(sizeX / 2) + i >= gridX-1 || posZ - (int)(sizeZ / 2) + j >= gridZ-1){
                        continue;
                    }
                  

                        if (grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].posibilitySpace.Count != 0)
                        {
                            if (grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].posibilitySpace[0].GetComponent<Modulescript>().weight == 0)
                            {
                                continue;
                            }
                        }
                    
                        grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].posibilitySpace.Clear();
                        grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].posibilitySpace.AddRange(modules);
                        Destroy(grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].instantiatedModule);
                    if(grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].IsInstantiated)
                    {
                        gM.size++;
                        grid[posX - (int)(sizeX / 2) + i, posY - (int)(sizeY / 2) + k, posZ - (int)(sizeZ / 2) + j].IsInstantiated = false;
                    }
                    
                        
                    
                }
            }
        }
      
    }


    public void Revert(int sizeX, int sizeY, int sizeZ)
    {


        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                for (int j = 0; j < sizeZ; j++)
                {
                    grid[xRandom + i, yRandom + k, zRandom + j].posibilitySpace.Clear();
                    grid[xRandom + i, yRandom + k, zRandom + j].posibilitySpace.Add(savedMiniGrid[i, k, j].posibilitySpace[0]);
                    grid[xRandom + i, yRandom + k, zRandom + j].IsInstantiated = false;
                    Destroy(grid[xRandom + i, yRandom + k, zRandom + j].instantiatedModule);
                }
            }
        }
    }


}
