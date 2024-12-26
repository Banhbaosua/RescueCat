using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager
{
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    public event Action<Finger> OnTouchDown = delegate { };
    public event Action<Vector2> OnTouchMove = delegate { };
    public event Action OnTouchUp = delegate { };
    public void Initialized()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += Touch_onFingerDown;
        Touch.onFingerUp += Touch_onFingerUp;
        Touch.onFingerMove += Touch_onFingerMove;
    }

    private void Touch_onFingerMove(Finger finger)
    {
        endTouchPos = finger.screenPosition;
        var direction = endTouchPos - startTouchPos;
        OnTouchMove?.Invoke(direction);
    }

    private void Touch_onFingerUp(Finger _)
    {
        startTouchPos = Vector2.zero; 
        endTouchPos = Vector2.zero;
        OnTouchUp?.Invoke();
    }

    private void Touch_onFingerDown(Finger finger)
    {
        startTouchPos = finger.screenPosition;
        OnTouchDown?.Invoke(finger);
    }

    public Vector2 GetDirection()
    {
        return startTouchPos - endTouchPos;
    }
}
