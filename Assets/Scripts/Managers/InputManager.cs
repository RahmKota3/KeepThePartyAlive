using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }

    public Action OnPickUpButtonPressed;
    public Action OnThrowButtonPressed;
    public Action OnGameRestartButtonPressed;

    public Action OnPlayButtonPressed;
    public Action OnPlayInfiniteButtonPressed;
    public Action OnControlsButtonPressed;
    public Action OnExitButtonPressed;
    public Action OnMenuButtonPressed;
    public Action OnRestartButtonPressed;

    void CheckForInput()
    {
        HorizontalAxis = Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("PickUp"))
            OnPickUpButtonPressed?.Invoke();

        if (Input.GetButtonDown("Throw"))
            OnThrowButtonPressed?.Invoke();


        // MainMenu
        if (Input.GetButtonDown("Play"))
            OnPlayButtonPressed?.Invoke();

        if (Input.GetButtonDown("PlayInfinite"))
            OnPlayInfiniteButtonPressed?.Invoke();

        if (Input.GetButtonDown("Controls"))
            OnControlsButtonPressed?.Invoke();

        if (Input.GetButtonDown("Exit"))
            OnExitButtonPressed?.Invoke();

        if (Input.GetButtonDown("Menu"))
            OnMenuButtonPressed?.Invoke();

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
