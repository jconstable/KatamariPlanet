using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    // Singleton, gross
    private static EventManager _instance;
	private static EventManager instance
    {
        get
        {
            if( _instance == null )
            {
                _instance = GameObject.FindObjectOfType<EventManager>();
                if( _instance != null )
                {
                    _instance.Setup();
                }
            }
            return _instance;
        }
    }

    public static void AddListener(string eventName, EventCallback action)
    {
        if( instance != null )
        {
            instance._AddListener(eventName, action);
        }
    }

    public static void RemoveListener( string eventName, EventCallback action )
    {
        if( instance != null )
        {
            instance._RemoveListener(eventName, action);
        }
    }

    public static void SendEvent( string eventName, object param )
    {
        if( instance != null )
        {
            instance._SendEvent(eventName, param);
        }
    }

    // The API for the callback function. Return true means stop processing this transaction.
    public delegate bool EventCallback( object param );
    
    // Dictionary to keep track of lists of listeners, by event name
    private Dictionary<string, List<EventCallback>> _listenersByEventName;

    private struct DeletePair
    {
        public string eventName;
        public EventCallback callback;
    }
    private List<DeletePair> _callbacksToRemoveAtEndOfFrame;


    private void Setup()
    {
        _listenersByEventName = new Dictionary<string, List<EventCallback>>();
        _callbacksToRemoveAtEndOfFrame = new List<DeletePair>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void LateUpdate()
    {
        if( _callbacksToRemoveAtEndOfFrame.Count > 0 )
        {
            for (int i = 0; i < _callbacksToRemoveAtEndOfFrame.Count; ++i)
            {
                DeletePair p = _callbacksToRemoveAtEndOfFrame[i];

                List<EventCallback> list = null;
                if (_listenersByEventName.TryGetValue(p.eventName, out list))
                {
                    list.Remove(p.callback);
                }
            }

            _callbacksToRemoveAtEndOfFrame.Clear();
        }
    }

    private void _AddListener( string eventName, EventCallback action )
    {
        Debug.Assert(!string.IsNullOrEmpty(eventName), "Null or empty event name given to EventManager::AddListener");
        if( !string.IsNullOrEmpty(eventName) )
        {
            // Get the list of listeners, or create it if needed
            List<EventCallback> list = null;
            if (!_listenersByEventName.TryGetValue(eventName, out list))
            {
                list = new List<EventCallback>();
                _listenersByEventName.Add(eventName, list);
            }

            list.Add(action);
        }
    }

    private void _RemoveListener( string eventName, EventCallback action )
    {
        _callbacksToRemoveAtEndOfFrame.Add(new DeletePair() { eventName = eventName, callback = action });
    }

    private void _SendEvent( string eventName, object param )
    {
        List<EventCallback> list = null;
        if (_listenersByEventName.TryGetValue(eventName, out list))
        {
            for( int i = 0; i < list.Count; ++i )
            {
                EventCallback cb = list[i];
                if( cb != null )
                {
                    // Return out if invoking the callback returns true.
                    if( cb(param) )
                    {
                        return;
                    }
                }
            }
        }
    }
}
