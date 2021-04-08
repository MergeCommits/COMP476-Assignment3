using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    private static InputManager singleton;

    private static List<GenericInput> inputs;
    
    private Gamepad gamepad;
    public static MovementInput movement;

    public static MouseInput mouse;
    public static ButtonInput pauseKey;

    private void Awake() {
        gamepad = Gamepad.current;

        if (singleton == null) {
            singleton = this;
        }
    }

    private void OnEnable() {
        if (inputs != null) { return; }

        inputs = new List<GenericInput>();

        mouse = AddInput(new MouseInput());

        movement = AddInput(new MovementInput(Keyboard.current.wKey, Keyboard.current.sKey,
            Keyboard.current.aKey, Keyboard.current.dKey, gamepad));
        pauseKey = AddInput(new ButtonInput(Keyboard.current.escapeKey, gamepad?.startButton));
    }

    private void Update() {
        foreach (GenericInput input in inputs) {
            input.Update();
        }
    }

    private void FixedUpdate() {
        foreach (GenericInput input in inputs) {
            input.FixedUpdate();
        }
    }

    /// <summary>
    /// Forces a FixedUpdate run of the inputs. Used for running inputs while game is paused.
    /// </summary>
    public static void ForceFixedUpdate() {
        singleton.FixedUpdate();
    }

    private static T AddInput<T>(T input) where T : GenericInput {
        inputs.Add(input);
        return input;
    }
}
