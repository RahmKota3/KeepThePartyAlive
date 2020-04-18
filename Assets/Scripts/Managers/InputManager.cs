using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }

    public Action OnRestartButtonPressed;

    void CheckForInput()
    {
        HorizontalAxis = Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Restart"))
            OnRestartButtonPressed?.Invoke();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckForInput();
    }
}
