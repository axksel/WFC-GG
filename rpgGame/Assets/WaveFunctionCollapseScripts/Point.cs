using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 position;
    public List<Point> connectedPoints = new List<Point>();
    public float offsetX;
    public float offsetZ;


    public Point(Vector3 pos, float offsetX, float offsetZ)
    {
        this.offsetX = offsetX;
        this.offsetZ = offsetZ;
        position = pos + new Vector3(offsetX, 0, offsetZ);
    }
}