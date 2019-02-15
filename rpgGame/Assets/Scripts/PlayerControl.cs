using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public GameObject joystick;
    private Vector2 dir;

    public float movementSpeed;

    UnityEngine.AI.NavMeshAgent agent;
    //public NavMeshSurface[] surfaces;



    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();



    }

// Update is called once per frame
void FixedUpdate()
    {
        dir = joystick.GetComponent<Joystick>().direction;
        //transform.position += new Vector3(dir.x, 0, dir.y).normalized * 0.001f * Mathf.Clamp(dir.magnitude, 0, 30) * movementSpeed;
        agent.destination = transform.position + new Vector3(dir.x, 0, dir.y).normalized;


    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }
}
