using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    private static Dictionary<string, Action<Dictionary<string, object>>> s_eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();

    // private static readonly EventManager s_eventManager = new EventManager();
    //
    // static EventManager()
    // {
    // }
    //
    // private EventManager()
    // {
    // }
    //
    // public static EventManager Instance => s_eventManager;

    public static void StartListening(string eventName, Action<Dictionary<string, object>> listener) 
    {
        Action<Dictionary<string, object>> thisEvent;
    
        if (s_eventDictionary.TryGetValue(eventName, out thisEvent)) 
        {
            thisEvent += listener;
            s_eventDictionary[eventName] = thisEvent;
        } 
        else 
        {
            thisEvent += listener;
            s_eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<Dictionary<string, object>> listener) 
    {
        // if (s_eventManager == null) return;
        
        Action<Dictionary<string, object>> thisEvent;
        
        if (s_eventDictionary.TryGetValue(eventName, out thisEvent)) 
        {
            thisEvent -= listener;
            s_eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, Dictionary<string, object> message) 
    {
        Action<Dictionary<string, object>> thisEvent = null;
          
        if (s_eventDictionary.TryGetValue(eventName, out thisEvent)) 
        {
            thisEvent?.Invoke(message);
        }
    }
}