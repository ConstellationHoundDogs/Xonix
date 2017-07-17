using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    
    public Action OnSwipeUp;
    public Action OnSwipeDown;
    public Action OnSwipeLeft;
    public Action OnSwipeRight;
    
	void Update ()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        
        if (touch.deltaPosition.x > 0)
        {
            if (OnSwipeRight != null)
            {
                OnSwipeRight();
            }
        }

        if (touch.deltaPosition.x < 0)
        {
            if (OnSwipeLeft != null)
            {
                OnSwipeLeft();
            }
        }

        if (touch.deltaPosition.y < 0)
        {
            if (OnSwipeDown != null)
            {
                OnSwipeDown();
            }
        }

        if (touch.deltaPosition.y > 0)
        {
            if (OnSwipeUp != null)
            {
                OnSwipeUp();
            }
        }

    }
}
