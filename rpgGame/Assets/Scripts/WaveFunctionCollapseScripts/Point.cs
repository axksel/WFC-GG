using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 position;
    public List<Point> connectedPoints = new List<Point>();
    public float offsetX;
    public float offsetZ;
    public Vector3 offsetPos;
    public int counter = 0;
    public int xIndex;
    public int yIndex;
   



    public Point(Vector3 pos, float offsetX, float offsetZ)
    {
        this.offsetX = offsetX;
        this.offsetZ = offsetZ;
        this.offsetPos = new Vector3(offsetX, 0, offsetZ);
        position = pos;
        

    }
}