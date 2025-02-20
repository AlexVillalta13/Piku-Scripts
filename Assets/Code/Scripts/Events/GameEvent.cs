using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    [ShowInInspector] private readonly HashSet<GameEventListener<T>> gameEventsList = new ();

    [ShowInInspector] private readonly List<object> gameObjectAttached = new List<object>();

    [Button(ButtonSizes.Medium)]
    public void Raise (T value, object objectListener)
    {
        // string path = AssetDatabase.GetAssetPath(this);
        // Debug.Log("GameObject: " + gameObjectAttached + ", script: " + ", SO: " + System.IO.Path.GetFileName(path));



        this.gameObjectAttached.Add(objectListener);
        foreach (GameEventListener<T> listener in gameEventsList)
        {
            listener.OnEventRaised(value);
        }
    }

    public void RegisterEventListener(GameEventListener<T> listener)
    {
        if(gameEventsList.Contains(listener) == false)
        {
            gameEventsList.Add(listener);
        }
    }

    public void UnregisterEventListener (GameEventListener<T> listener)
    {
        if(gameEventsList.Contains (listener) == false)
        {
            gameEventsList.Remove(listener);
        }
    }
}

public readonly struct Empty {}
[CreateAssetMenu(menuName = "Events/Game event", fileName = "Game event")]
public class GameEvent : GameEvent<Empty> { }