using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public interface GenericInput {
    void Update();
    void FixedUpdate();
}

public class ButtonInput : GenericInput {
    /// <summary>
    /// If at any point between this tick and the last, was a button down.
    /// </summary>
    private bool wasDownBetweenFixedUpdates = false;

    /// <summary>
    /// Whether this Update run is the first since one or more FixedUpdate calls.
    /// </summary>
    private bool finishedFixedUpdate = false;

    public bool isHit { get; private set; } = false;
    public bool isDown { get; private set; } = false;

    public bool isUp {
        get { return !isDown; }
    }

    private readonly ButtonControl[] buttons;

    public ButtonInput(params ButtonControl[] btns) {
        List<ButtonControl> butt = new List<ButtonControl>();
        foreach (ButtonControl btn in btns) {
            if (btn != null) {
                butt.Add(btn);
            }
        }

        buttons = butt.ToArray();
    }

    public void Update() {
        if (finishedFixedUpdate) {
            wasDownBetweenFixedUpdates = false;
            finishedFixedUpdate = false;
        }

        if (!wasDownBetweenFixedUpdates) {
            foreach (ButtonControl btn in buttons) {
                if (btn.isPressed) {
                    wasDownBetweenFixedUpdates = true;
                    break;
                }
            }
        }
    }

    public void FixedUpdate() {
        isHit = false;
        if (wasDownBetweenFixedUpdates) {
            if (!isDown) {
                isHit = true;
            }

            isDown = true;
        } else {
            isDown = false;
        }

        finishedFixedUpdate = true;
    } 
}

public class MouseInput : GenericInput {
    public Vector2 mousePosition { get; private set; }
    public ButtonInput mouse1 { get; private set; }

    public MouseInput() {
        mousePosition = Vector2.zero;
        mouse1 = new ButtonInput(Mouse.current.leftButton);
    }

    public void Update() {
        if (Mouse.current == null) { return; }

        mousePosition = new Vector2(
            Mouse.current.position.x.ReadValue(),
            Mouse.current.position.y.ReadValue()
        );
        
        mouse1.Update();
    }

    public void FixedUpdate() {
        mouse1.FixedUpdate();
    }

    public bool IsOverGameWindow() {
        return !(0 > mousePosition.x
                 || 0 > mousePosition.y
                 || Screen.width < mousePosition.x
                 || Screen.height < mousePosition.y);
    }
}

public class StickInput : GenericInput {
    private readonly StickControl stick;
    public Vector2 stickPosition { get; private set; }

    public StickInput(StickControl stick) {
        this.stick = stick;
        stickPosition = Vector2.zero;
    }

    public void Update() {
        stickPosition = stick.ReadValue();
    }

    public void FixedUpdate() {
        Update();
    }
}
