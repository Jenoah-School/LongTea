﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
        {
            if (backgroundImage != null)
            {
                backgroundImage.DOFade(.2f, 0.3f);
            }
            else
            {
                background.gameObject.SetActive(false);
            }
            if (handleImage != null)
            {
                handleImage.DOFade(.2f, 0.3f);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            if (backgroundImage != null)
            {
                backgroundImage.DOFade(1f, 0.3f);
            }
            else
            {
                background.gameObject.SetActive(true);
            }
            if (handleImage != null)
            {
                handleImage.DOFade(1f, 0.3f);
            }
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            if (backgroundImage != null)
            {
                backgroundImage.DOFade(.2f, 0.3f);
            }
            else
            {
                background.gameObject.SetActive(false);
            }
            if (handleImage != null)
            {
                handleImage.DOFade(.2f, 0.3f);
            }
        }

        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }