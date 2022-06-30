using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Vector2 startPos;

    public float minSwipeDistY = 50f;
    public float minSwipeDistX = 50f;

    private bool firstSwipe = false;

    public string direction = "";

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
                    direction = "Right";

                    firstSwipe = false;
                }
                else if (swipeValue < 0)
                {
                    direction = "Left";

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
                    direction = "Up";

                    firstSwipe = false;
                }
                else if (swipeValue < 0)
                {
                    direction = "Down";

                    firstSwipe = false;
                }
            }
        }

        Invoke("OnReset", 0.25f);
    }

    private void OnReset()
    {
        direction = "";
    }
}
