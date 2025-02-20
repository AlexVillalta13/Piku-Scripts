using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T> : MonoBehaviour
{
    [SerializeField] GameEvent<T> gameEvent;
    [SerializeField] UnityEvent<T> unityEvent;

    private void OnEnable()
    {
        gameEvent.RegisterEventListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterEventListener(this);
    }

    public void OnEventRaised(T value)
    {
        //Debug.Log(": " + gameEvent.name);
        unityEvent.Invoke(value);
    }
}

public class GameEventListener : GameEventListener<Empty> {}