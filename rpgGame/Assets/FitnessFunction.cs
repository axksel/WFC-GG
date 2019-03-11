using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessFunction : MonoBehaviour
{

    public float Doorfitness = 1;
     float tmpFitness = 100;
    // Start is called before the first frame update


    public bool CalculateDoorFitness(int index)
    {
       
        if (index > Doorfitness)
        {

            return true;
        }
        return false;
    }
    public bool IsImproved(int fitness)
    {
        
        if (tmpFitness > fitness)
        {
            
            tmpFitness = fitness;
            return true;
        }
        else
        {
            return false;
        }
        
    }

}
