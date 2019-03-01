using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Joystick : MonoBehaviour
{

    public GameObject joystickOuter;
    public GameObject joystickInner;
    public GameObject aimArrow;
    public GameObject spellCooldown;
    public bool attackJoystick;
    public Vector2Event weaponFired;
    public Vector2 direction;

    private float radius;
    private float targetAlpha;
    private float FadeRate = 5;

    bool joystickPressed = false;
    int id;
    GameObject player;
    PlayerControl playerControl;

    float spellTimer;
    float spellFirerate = 2f;
    float spellCooldownFill;
    float i = 0f;

    void Start()
    {
        radius = joystickInner.GetComponentInParent<Canvas>().scaleFactor * 40;
        spellCooldownFill = spellCooldown.GetComponent<Image>().fillAmount;
        spellTimer = -100;
        player = GameObject.FindGameObjectWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
    }

    void Update()
    {
        Color curColor = joystickInner.GetComponent<Image>().color;
        float alphaDiff = Mathf.Abs(curColor.a - this.targetAlpha);
        if (alphaDiff > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.FadeRate * Time.deltaTime);
            joystickInner.GetComponent<Image>().color = curColor;
        }

        if (attackJoystick)
        {
            aimArrow.transform.position = player.transform.position + new Vector3(0, 0.2f, 0);
            Color curColor2 = aimArrow.GetComponentInChildren<MeshRenderer>().material.color;
            float alphaDiff2 = Mathf.Abs(curColor.a - this.targetAlpha);
            if (alphaDiff > 0.0001f)
            {
                curColor2.a = Mathf.Lerp(curColor2.a, targetAlpha, this.FadeRate * Time.deltaTime);
                aimArrow.GetComponentInChildren<MeshRenderer>().material.color = curColor2;
            }
        }

        if (i < 1.0f)
        {
            i += Time.deltaTime / spellFirerate;
        }

        spellCooldownFill = Mathf.Lerp(1, 0f, i);
        spellCooldown.GetComponent<Image>().fillAmount = spellCooldownFill;

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0) && Vector2.Distance(Input.mousePosition, joystickOuter.transform.position) < 50)
        {
            direction = Input.mousePosition - joystickOuter.transform.position;
            joystickPressed = true;
            FadeIn();
        }

        if (Input.GetMouseButton(0) && joystickPressed)
        {
            direction = Input.mousePosition - joystickOuter.transform.position;
            joystickInner.transform.position = joystickOuter.transform.position + Vector3.ClampMagnitude(direction, radius);
            joystickInner.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        }

        if (Input.GetMouseButtonUp(0) && joystickPressed)
        {
            joystickPressed = false;
            FadeOut();
            direction = Vector2.zero;
        }

#endif



#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            int tapCount = Input.touchCount;

            for (int i = 0; i < tapCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (Vector2.Distance(touch.position, joystickOuter.transform.position) < 50)
                        {
                            id = touch.fingerId;
                            joystickInner.SetActive(true);
                            this.direction = touch.position - new Vector2(joystickOuter.transform.position.x, joystickOuter.transform.position.y);
                            joystickPressed = true;
                            FadeIn();
                        }
                        break;

                    case TouchPhase.Moved:
                        if (joystickPressed && id == touch.fingerId)
                        {
                            this.direction = touch.position - new Vector2(joystickOuter.transform.position.x, joystickOuter.transform.position.y);
                            joystickInner.transform.position = joystickOuter.transform.position + Vector3.ClampMagnitude(this.direction, radius);
                            joystickInner.transform.eulerAngles = new Vector3(0, 0,Vector2.SignedAngle(Vector2.right, this.direction));
                            if (attackJoystick)
                            { 
                                aimArrow.transform.eulerAngles = new Vector3(90, Vector2.SignedAngle(this.direction, Vector2.up), 0);
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        if (joystickPressed && id == touch.fingerId)
                        {
                            joystickInner.SetActive(false);
                            FadeOut();
                            this.direction = Vector2.zero;
                            joystickPressed = false;
                        }
                        break;
                }
            }
        }      
#endif
    }

    public void FadeOut()
    {
        if(spellTimer + spellFirerate < Time.time && attackJoystick)
        {
            spellTimer = Time.time;
            i = 0f;
            spellCooldownFill = 1;
            spellCooldown.GetComponent<Image>().fillAmount = spellCooldownFill;
            playerControl.anim.SetBool("castSpell", true);
            weaponFired.Invoke(this.direction);
        }
        this.targetAlpha = 0.0f;
    }

    public void FadeIn()
    {
        this.targetAlpha = 1.0f;
    }
}
