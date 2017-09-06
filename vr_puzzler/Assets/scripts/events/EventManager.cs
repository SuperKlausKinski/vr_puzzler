using System.Collections.Generic;
using UnityEngine.Events;
using VRPuzzler.Templates;
using UnityEngine;


public class EventManager : Singleton<EventManager>
{

    private Dictionary<string, UnityEvent> m_eventDictionary;

    public override void Awake()
    {
        base.Awake();
        m_eventDictionary = new Dictionary<string, UnityEvent>();
    }

    public void StartListening(string _eventName, UnityAction _listener)
    {
        UnityEvent _thisEvent = null;
        if (m_eventDictionary.TryGetValue(_eventName, out _thisEvent))
        {
            _thisEvent.AddListener(_listener);
        }
        else
        {
            _thisEvent = new UnityEvent();
            _thisEvent.AddListener(_listener);
            m_eventDictionary.Add(_eventName, _thisEvent);
        }
    }

    public void StopListening(string _eventName, UnityAction _listener)
    {
        UnityEvent _thisEvent = null;
        if (m_eventDictionary.TryGetValue(_eventName, out _thisEvent))
        {
            _thisEvent.RemoveListener(_listener);
        }
    }
    public void InvokeEvent(string _eventName)
    {
       
        UnityEvent _thisEvent = null;
        Debug.Log("EventName: " + _eventName);
        if (m_eventDictionary.TryGetValue(_eventName, out _thisEvent))
        {
            Debug.Log(_thisEvent);
            _thisEvent.Invoke();
        }
    }

}
