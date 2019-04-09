using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGManager : MonoBehaviour
{

    public int xSize =2;
    public int ySize =2;
    public GridManager gm;
    public int timeCounter=0;
    int time =0;

    [Range(1, 1000)]
    public int maxNumberOfNodes =100; //nMax

    [Range(0.01f, 9.91f)]
    public float neighbourhoodRange = 2;//Sigma

    [Range(0, 200)]
    public int epochSize =50;//lampda

    [Range(0.0f, 0.1f)]
    public float learningRate=0.03f;//learning rate


    public  Point[,] points;
    public List<Point> distributionPoints = new List<Point>();
    public bool growthPhase = true;
    public bool fineTunePhase = false;

    public Point randomPoint;
    Point winningPoint;
    Point mostWinningPoint;
    Point mostDistantPoint;


    void Start()
    {
        points = new Point[maxNumberOfNodes, maxNumberOfNodes];

        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < ySize; k++)
            {

                points[i, k] = distributionPoints[Random.Range(0,distributionPoints.Count)];
                points[i, k].xIndex = i;
                points[i, k].yIndex = k;
            }
        }

        
    }

    private void Update()
    {
        if (growthPhase)
        {
            Iterate();
            timeCounter++;
        }
        else if (fineTunePhase)
        {
            FineTune();
            timeCounter++;
        }
        else
        {
            Debug.Log("Done");
            gm = GetComponent<GridManager>();
            int index = 0;
            gm.gridX = xSize-1;
            gm.gridZ = ySize-1;
            gm.InitializePoints();

            //for (int i = 0; i < xSize; i++)
            //{
            //    for (int k = 0; k < ySize; k++)
            //    {

            //        gm.allPoints[index].offsetPos = points[i, k].position;
            //        index++;
            //    }
            //}
            //Debug.Log(index);
            gm.Chiseling();
            Destroy(this);
        }

        
    }

    public void Iterate()
    {
        winningPoint = CalculateWinner();
        time++;
        winningPoint.counter++;
        Adapt();
        if (epochSize * xSize * ySize < time)
        {
           
            mostWinningPoint = FindPointWithMostWins();
            mostDistantPoint = CheckNeighbours();
            CheckRowVsColumns();
            time = 0;
            ResetCount();
            CheckSize();
        }



    }


    public void FineTune()
    {
        winningPoint = CalculateWinner();
        time++;
        winningPoint.counter++;
        Adapt();
        if (epochSize * xSize * ySize < time)
        {
            fineTunePhase = false;
        }
    }

    public void CheckSize()
    {

        if (xSize * ySize > maxNumberOfNodes)
        {
            growthPhase = false;
            fineTunePhase = true;
            Debug.Log("Now Tuning");
        }
    }


    public void ResetCount()
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < ySize; k++)
            {
                if (points[i, k] != null)
                {
                    points[i, k].counter = 0;
                }
            }

        }
    }

    public void CheckRowVsColumns()
    {

        if (mostWinningPoint.xIndex == mostDistantPoint.xIndex)
        {
            InsertRow();
        }
        else
        {
            InsertColumn();
        }

      
    }

    public void InsertColumn()
    {
       int mod = 0;
       if( mostWinningPoint.xIndex > mostDistantPoint.xIndex)
        {
            mod = 1;
        }

        for (int i = xSize - 1; i >= mostWinningPoint.xIndex-mod; i--)
        {
            for (int k = 0; k < ySize; k++)
            {
                points[i + 1, k] = DeepCopy.DeepCopyPoint(points[i, k]);
                points[i + 1, k].xIndex++;
            }
        }

        int index = mostWinningPoint.xIndex + 1 - mod;
        for (int i = 0; i < ySize; i++)
        {
            points[index, i].position = Vector3.Lerp(points[index, i].position, points[index + 1, i].position, 0.5f);
        }

        xSize++;
    }

    public void InsertRow()
    {
        int mod = 0;
        if (mostWinningPoint.yIndex > mostDistantPoint.yIndex)
        {
            mod = 1;
        }

        for (int i = ySize - 1; i >= mostWinningPoint.yIndex - mod; i--)
        {
            for (int k = 0; k < xSize; k++)
            {
                points[k, i + 1] = DeepCopy.DeepCopyPoint(points[k, i]);
                points[k, i + 1].yIndex++;
            }
        }

        int index = mostWinningPoint.yIndex + 1 - mod;
        for (int i = 0; i < xSize; i++)
        {
            points[i, index].position = Vector3.Lerp(points[i, index].position, points[i, index + 1].position, 0.5f);
        }
        ySize++;
    }


    public Point CheckNeighbours()
    {
        float tmpDist=0;
        Point tmpPoint = new Point(new Vector3(0, 0, 0),0,0);

        try
        {
            float tmpTmpDist = Distance(mostWinningPoint.position, points[mostWinningPoint.xIndex + 1, mostWinningPoint.yIndex].position);
            if (tmpTmpDist > tmpDist)
            {
                tmpPoint = points[mostWinningPoint.xIndex + 1, mostWinningPoint.yIndex];
                tmpDist = tmpTmpDist;
            }
        }
        catch (System.IndexOutOfRangeException) { }
        catch (System.NullReferenceException) { }
        try
        {
            float tmpTmpDist = Distance(mostWinningPoint.position, points[mostWinningPoint.xIndex - 1, mostWinningPoint.yIndex].position);
            if (tmpTmpDist > tmpDist)
            {
                tmpPoint = points[mostWinningPoint.xIndex - 1, mostWinningPoint.yIndex];
                tmpDist = tmpTmpDist;
            }
        }
        catch (System.IndexOutOfRangeException) { }
        catch (System.NullReferenceException) { }

        try
        {
            float tmpTmpDist = Distance(mostWinningPoint.position, points[mostWinningPoint.xIndex, mostWinningPoint.yIndex+1].position);
            if (tmpTmpDist > tmpDist)
            {
                tmpPoint = points[mostWinningPoint.xIndex , mostWinningPoint.yIndex+1];
                tmpDist = tmpTmpDist;
            }
        }
        catch (System.IndexOutOfRangeException) { }
        catch (System.NullReferenceException) { }

        try
        {
            float tmpTmpDist = Distance(mostWinningPoint.position, points[mostWinningPoint.xIndex , mostWinningPoint.yIndex-1].position);
            if (tmpTmpDist > tmpDist)
            {
                tmpPoint = points[mostWinningPoint.xIndex , mostWinningPoint.yIndex-1];
                tmpDist = tmpTmpDist;
            }
        }
        catch (System.IndexOutOfRangeException) { }
        catch (System.NullReferenceException) { }

      
        return tmpPoint;
    }


    public Point FindPointWithMostWins()
    {
        int tmpCount=0;
        Point tmpPoint = new Point(new Vector3(0, 0, 0),0,0);

            for (int i = 0; i < xSize; i++)
            {
                for (int k = 0; k < ySize; k++)
                {
                    
                    if (points[i,k].counter > tmpCount)
                    {
                        tmpPoint = points[i, k];
                        tmpCount = points[i, k].counter;
                    }

                }
            }

        
        return tmpPoint;
    }


    public void Adapt()
    {


        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < ySize; k++)
            {
                Vector3 direction = randomPoint.position - points[i, k].position;
                points[i, k].position += learningRate*AdaptationStrength(winningPoint, points[i, k]) * direction;
                

            }
        }

    }

    public Point CalculateWinner()
    {
        randomPoint = distributionPoints[Random.Range(0, distributionPoints.Count)];
        Point tmpPoint = new Point(new Vector3(0,0,0),0,0);
        float tmpDistance=10000f;
        float distance=0;

        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < ySize; k++)
            {
                distance = Vector3.Distance(points[i, k].position, randomPoint.position);
                if (distance< tmpDistance)
                {
                    tmpPoint = points[i, k];
                    tmpDistance = distance;
                }

            }
        }
        
        return tmpPoint;
    }


    public float AdaptationStrength(Point point1, Point point2)
    {
        float d = Distance(point1, point2);
        return Mathf.Exp(-d * d / (2 * neighbourhoodRange * neighbourhoodRange));
    }

    public float Distance(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Abs(vector1.x - vector2.x) + Mathf.Abs(vector1.z - vector2.z);
    }

    public float Distance(Point vector1, Point vector2)
    {
        return Mathf.Abs(vector1.xIndex - vector2.xIndex) + Mathf.Abs(vector1.yIndex - vector2.yIndex);

    }


    private void OnDrawGizmos()
    {

        for (int i = 0; i < distributionPoints.Count; i++)
        {
            Gizmos.DrawSphere(distributionPoints[i].position, 0.5f);
        }
        Gizmos.color =Color.red;

        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < ySize; k++)
            {
               
                Gizmos.DrawSphere(points[i, k].position, (i+k)/2+0.1f);

            }
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(randomPoint.position, 0.6f);


        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < ySize; k++)
            {
                if (points[i+1, k] != null)
                {
                    Gizmos.DrawLine(points[i, k].position, points[i + 1, k].position);
                }
                if (i - 1 > 0)
                {
                    Gizmos.DrawLine(points[i, k].position, points[i - 1, k].position);
                }
                if (points[i, k + 1] != null)
                {
                    Gizmos.DrawLine(points[i, k].position, points[i, k + 1].position);
                }
                if (k - 1 > 0)
                {
                    Gizmos.DrawLine(points[i, k].position, points[i, k - 1].position);
                }

            }
        }
    }


}
