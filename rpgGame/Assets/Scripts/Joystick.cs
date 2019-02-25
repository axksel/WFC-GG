using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Joystick : MonoBehaviour
{

    public GameObject joystickOuter;
    public GameObject joystickInner;
    public Vector2 direction;
    private float radius;
    bool joystickPressed = false;
    public Vector2Event weaponFired;
    

    private float targetAlpha;
    public float FadeRate;

    private int screenWidth;
    private int screenHeight;


    void Start()
    {
        radius = joystickInner.GetComponentInParent<Canvas>().scaleFactor * 40;
      
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

//#if UNITY_EDITOR

//        if (Input.GetMouseButtonDown(0) && Vector2.Distance(Input.mousePosition, joystickOuter.transform.position) < 50)
//        {
//            direction = Input.mousePosition - joystickOuter.transform.position;
//            joystickPressed = true;
//            FadeIn();
//        }

//        if (Input.GetMouseButton(0) && joystickPressed)
//        {
//            direction = Input.mousePosition - joystickOuter.transform.position;
//            joystickInner.transform.position = joystickOuter.transform.position + Vector3.ClampMagnitude(direction, radius);
//            joystickInner.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, direction));
//        }

//        if (Input.GetMouseButtonUp(0) && joystickPressed)
//        {
//            joystickPressed = false;
//            FadeOut();
//            direction = Vector2.zero;
//        }

//#endif



#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            int tapCount = Input.touchCount;
            

            for (int i = 0; i < tapCount; i++)
            {
                int[] ids = new int[tapCount];
                Touch touch = Input.GetTouch(i);
                Debug.Log(Vector2.Distance(touch.position, joystickOuter.transform.position));
                switch (touch.phase)
                {
                   
                    case TouchPhase.Began:

                        if (Vector2.Distance(touch.position, joystickOuter.transform.position) < 50)
                        {
                            ids[i] = touch.fingerId;
                            joystickInner.SetActive(true);
                            this.direction = touch.position - new Vector2(joystickOuter.transform.position.x, joystickOuter.transform.position.y);
                            joystickPressed = true;
                            FadeIn();
                        }
                        break;

                    case TouchPhase.Moved:
                        if (joystickPressed && touch.fingerId == ids[i] && Vector2.Distance(touch.position, joystickOuter.transform.position) < 100)
                        {
                            this.direction = touch.position - new Vector2(joystickOuter.transform.position.x, joystickOuter.transform.position.y);
                            joystickInner.transform.position = joystickOuter.transform.position + Vector3.ClampMagnitude(this.direction, radius);
                            joystickInner.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, this.direction));
                        }
                        break;

                    case TouchPhase.Ended:
                        if (joystickPressed && touch.fingerId == ids[i])
                        {
                            joystickInner.SetActive(false);
                            weaponFired.Invoke(this.direction);
                            this.direction = Vector2.zero;
                            FadeOut();
                        }
                        break;
                }
            }
        }
            

#endif
    }

    public void FadeOut()
    {
        
        this.targetAlpha = 0.0f;
    }

    public void FadeIn()
    {
        this.targetAlpha = 1.0f;
    }
}
