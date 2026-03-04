using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;

    public bool ShowDebug;

    protected virtual void Start()
    {
        if (CurrentState == null)
        {
            Debug.LogWarning("There are no states selected to start with.");
            return;
        }
        CurrentState.EnterState();
    }

    protected virtual void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();
        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    protected virtual void FixedUpdate()
    {
        CurrentState.FixedUpdateState();
    }

    public void TransitionToState(EState statekey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[statekey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CurrentState.OnTriggerEnter2D(collision);
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        CurrentState.OnTriggerStay2D(collision);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        CurrentState.OnTriggerExit2D(collision);
    }
    
    private void OnGUI()
    {
        if (!ShowDebug) return;

        // Scale based on screen resolution
        float screenScale = Screen.height / 720f; // 720p baseline
        int fontSize = Mathf.RoundToInt(18 * screenScale);

        GUIStyle style = new(GUI.skin.label)
        {
            fontSize = fontSize,
            normal = { textColor = Color.white },
            fontStyle = FontStyle.Bold
        };

        // Message to display
        string message = CurrentState == null
            ? "Current State: NULL"
            : $"Current State: {CurrentState.StateKey}";

        // Prepare label background
        Rect labelRect = new Rect(10, 10, Screen.width * 0.6f, fontSize + 10);
        Color originalColor = GUI.color;
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.Box(labelRect, GUIContent.none); // Background box
        GUI.color = originalColor;

        // Draw label
        GUI.Label(labelRect, message, style);
    }
}
