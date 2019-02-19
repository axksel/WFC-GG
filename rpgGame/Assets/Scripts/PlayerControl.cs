using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    private GameObject joystick;
    private Vector2 dir;

    UnityEngine.AI.NavMeshAgent agent;

    Animator anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    public EnemyList enemiesInRange;

    RaycastHit hitInfo = new RaycastHit();

    Vector2 velocity = Vector2.zero;
    bool shouldMove;
    Button interactButton;


    void Start()
    {
        interactButton = GameObject.FindGameObjectWithTag("interactButton").GetComponent<Button>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updatePosition = false;

        joystick = GameObject.FindGameObjectWithTag("Joystick");
    }

    private void Update()
    {
        if (Input.GetKey("space"))
        {
            anim.SetBool("swordSlash", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("swordSlash"))
        {
            anim.SetBool("swordSlash", false);
        }

        dir = joystick.GetComponent<Joystick>().direction;

        agent.speed = Mathf.Clamp(dir.magnitude, 0, 30) / 8;
        agent.destination = transform.position + new Vector3(dir.x, 0, dir.y).normalized / 5;
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        if (dir == Vector2.zero)
        {
            //anim.SetLayerWeight(1, 0);
            //anim.SetLayerWeight(2, 1);
            shouldMove = false;
        }
        else
        {
            shouldMove = true;
            //anim.SetLayerWeight(2, 0);
            //anim.SetLayerWeight(1, 0.5f);
        }


        // Update animation parameters
        anim.SetBool("shouldMove", shouldMove);
        anim.SetFloat("velX", velocity.x);
        anim.SetFloat("velY", velocity.y);

        GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    void FixedUpdate()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Interactable")
        {
            interactButton.interactable = true;
            interactButton.GetComponentInChildren<Text>().text = other.gameObject.GetComponent<IsInteracable>().ReturnName();
            interactButton.onClick.AddListener(other.gameObject.GetComponent<IsInteracable>().Interact);

        }

        if (other.gameObject.tag == "enemy")
        {
            EnemyIO tmp = other.GetComponent<EnemyIO>();
            tmp.setIndex(enemiesInRange.list.Count);
            enemiesInRange.list.Add(other.gameObject);
            

        }


    }

    private void OnTriggerExit(Collider other)
    {


        if (other.gameObject.tag == "Interactable")
        {
            interactButton.interactable = false;
            interactButton.onClick.RemoveAllListeners();

        }
        if (other.gameObject.tag == "enemy")
        {
            EnemyIO tmp = other.GetComponent<EnemyIO>();
            tmp.getIndex();
            enemiesInRange.list.RemoveAt(tmp.getIndex());
            
        }

    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
        anim.ApplyBuiltinRootMotion();
    }
}
