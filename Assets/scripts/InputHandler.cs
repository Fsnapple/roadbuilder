﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event System.EventHandler<Vector3> OnClick;
    public event System.EventHandler<Vector3> OnDragStart;
    public event System.EventHandler<Vector3> OnDragEnd;

    public event System.EventHandler OnUndoPressed;
    
    float pressingTime;
    bool dragging = false;
    float y = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pressingTime += Time.deltaTime;
        }
        else
        {
            pressingTime = 0f;
        }
        if (pressingTime > 0.8f && !dragging)
        {
            dragging = true;
            OnDragStart(this, MagnetMousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                dragging = false;
                OnDragEnd(this, MagnetMousePosition);
            }
            else
            {
                OnClick(this, MagnetMousePosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            y = y + 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            y = y - 1.0f;
        }

        if (Input.GetKey(KeyCode.LeftApple) && Input.GetKeyDown(KeyCode.Z))
        {
            OnUndoPressed(this, null);
        }
    }

    public Vector3 MousePosition
    {
        get
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane y_0 = new Plane(Vector3.up, Vector3.up * y);
            y_0.Raycast(ray, out float enter);
            return ray.GetPoint(enter);
        }
    }

    public Vector3 MagnetMousePosition
    {
        get
        {
            RoadPositionRecords.QueryNodeOr3DCurve(MousePosition, out Vector3 position);
            return position;
        }
    }

    public void SwitchDragListenerTo(System.EventHandler<Vector3> handler)
    {
        if (dragging)
        {
            OnDragEnd(this, MagnetMousePosition);
            pressingTime = 0f;
            dragging = false;
        }
        OnDragStart = handler;
        OnDragEnd = handler;
    }

    public void SwitchClickListenerTo(System.EventHandler<Vector3> handler)
    {
        OnClick = handler;
    }
}