using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageParser : MonoBehaviour
{

    public Texture2D image;
    public bool[,] pixelValues;
    public List<Point> distributionPoints = new List<Point>();
    public GGManager gm;


    // Start is called before the first frame update
    void Awake()
    {

        gm = GetComponent<GGManager>();
        pixelValues = new bool[image.width, image.height];

        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                if (image.GetPixel(i, j).grayscale < 0.40f)
                {
                    distributionPoints.Add(new Point(new Vector3(i, 0, j), 0, 0));
                }
                if (image.GetPixel(i, j).grayscale < 0.60f)
                {
                    distributionPoints.Add(new Point(new Vector3(i, 0, j), 0, 0));
                }
                if (image.GetPixel(i, j).grayscale < 0.80f)
                {
                    distributionPoints.Add(new Point(new Vector3(i, 0, j), 0, 0));
                }
                if (image.GetPixel(i, j).grayscale < 0.95f)
                {
                    distributionPoints.Add(new Point(new Vector3(i, 0, j), 0, 0));
                }
            }
        }
        gm.distributionPoints.AddRange(distributionPoints);
    }

 
}
