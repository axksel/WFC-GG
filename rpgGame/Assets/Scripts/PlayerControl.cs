using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    private GameObject joystick;
    private GameObject joystickAttack;
    private Vector2 dir;
    private Vector2 tempDir;
    private Transform chest;
    UnityEngine.AI.NavMeshAgent agent;

    [HideInInspector]
    public Animator anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    public GameObjectList enemiesInRange;
    RaycastHit hitInfo = new RaycastHit();
    Vector2 velocity = Vector2.zero;
    bool shouldMove;
    Button interactButton;

    void Start()
    {
        interactButton = GameObject.FindGameObjectWithTag("interactButton").GetComponent<Button>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        joystick = GameObject.FindGameObjectWithTag("Joystick");
        joystickAttack = GameObject.FindGameObjectWithTag("joystickAttack");
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
    }

    private void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("swordSlash"))
        {
            anim.SetBool("swordSlash", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("castSpell"))
        {
            shouldMove = false;
            anim.SetBool("shouldMove", shouldMove);
            anim.SetBool("castSpell", false);
        }
        if (shouldMove)
        {
            agent.speed = Mathf.Clamp(dir.magnitude, 0, 30) / 8;
            agent.destination = transform.position + new Vector3(dir.x, 0, dir.y).normalized / 5;
 
            velocity = RotateVector(dir, Vector2.SignedAngle( new Vector3(transform.forward.x, transform.forward.z), new Vector3(Vector3.forward.x, Vector3.forward.z)));
            velocity.Normalize();
        }
        dir = joystick.GetComponent<Joystick>().direction;

        if (dir == Vector2.zero)
        {
            shouldMove = false;
        }
        else
        {
            shouldMove = true;
        }

        anim.SetBool("shouldMove", shouldMove);
        anim.SetFloat("velX", velocity.x);
        anim.SetFloat("velY", velocity.y);

        GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
        
    }

    void LateUpdate()
    {
        if(joystickAttack.GetComponent<Joystick>().direction.x != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(joystickAttack.GetComponent<Joystick>().direction.x, -10f, joystickAttack.GetComponent<Joystick>().direction.y));
            chest.rotation = targetRotation;
            tempDir = joystickAttack.GetComponent<Joystick>().direction;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("castSpell"))
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tempDir.x, -45f, tempDir.y));
            chest.rotation = targetRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            interactButton.interactable = true;
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = other.gameObject.GetComponent<IsInteracable>().ReturnName();
            interactButton.onClick.AddListener(other.gameObject.GetComponent<IsInteracable>().Interact);
        }

        if (other.gameObject.tag == "enemy")
        {
            enemiesInRange.list.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            interactButton.interactable = false;
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
            interactButton.onClick.RemoveAllListeners();
        }
        if (other.gameObject.tag == "enemy")
        {
            enemiesInRange.list.Remove(other.gameObject); 
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
        transform.position = agent.nextPosition;
        anim.ApplyBuiltinRootMotion();
    }
}
