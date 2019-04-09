using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeepCopy {

    public static Point DeepCopyPoint(Point p)
    {
        Point tmp = new Point(p.position,p.offsetX,p.offsetZ);
        tmp.xIndex = p.xIndex;
        tmp.yIndex = p.yIndex;
        tmp.offsetPos = p.offsetPos;

        return tmp;

    }


}
  



