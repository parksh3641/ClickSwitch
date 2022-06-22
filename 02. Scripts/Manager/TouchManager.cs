using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Vector2 startPos;

    public float minSwipeDistY = 50f;
    public float minSwipeDistX = 50f;

    private bool firstSwipe = false;


    void Update()
    {
        if (Time.timeScale == 0) { InputButtonUp(); return; }
    }

    public void InputButtonUp()
    {
        firstSwipe = false;
    }

    public void InputButtonDown()
    {
        startPos = Input.mousePosition;
        firstSwipe = true;
    }

    public void InputButtonStay()
    {
        if (!firstSwipe) return;
        float swipeDistHorizontal = Mathf.Abs(Input.mousePosition.x - startPos.x);
        float swipeDistVertical = Mathf.Abs(Input.mousePosition.y - startPos.y);
        if (swipeDistHorizontal > swipeDistVertical)
        {
            if (swipeDistHorizontal > minSwipeDistX)
            {
                float swipeValue = Input.mousePosition.x - startPos.x;
                if (swipeValue > 0)
                {
                    Debug.Log("오른쪽");

                    firstSwipe = false;
                }
                else if (swipeValue < 0)
                {
                    Debug.Log("왼쪽");

                    firstSwipe = false;
                }
            }
        }
        else
        {
            if (swipeDistVertical > minSwipeDistY)
            {
                float swipeValue = Input.mousePosition.y - startPos.y;
                if (swipeValue > 0)
                {
                    Debug.Log("위쪽");

                    firstSwipe = false;
                }
                else if (swipeValue < 0)
                {
                    Debug.Log("아래쪽");

                    firstSwipe = false;
                }
            }
        }

        Invoke("Reset", 0.25f);
    }

    private void Reset()
    {
        firstSwipe = false;
    }
}
