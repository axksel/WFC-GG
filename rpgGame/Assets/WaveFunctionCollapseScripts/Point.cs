using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 position;
    public List<Point> connectedPoints = new List<Point>();

    public Point(Vector3 pos)
    {
        position = pos;
    }
}