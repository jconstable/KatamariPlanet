using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {

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


    public void Setup()
    {
        _listenersByEventName = new Dictionary<string, List<EventCallback>>();
        _callbacksToRemoveAtEndOfFrame = new List<DeletePair>();
    }

    public void Teardown()
    {
        _listenersByEventName.Clear();
        _callbacksToRemoveAtEndOfFrame.Clear();

        _listenersByEventName = null;
        _callbacksToRemoveAtEndOfFrame = null;
    }

    public void LateUpdate(float dt)
    {
        CleanupRemovedCallbacks();
    }

    void CleanupRemovedCallbacks()
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

    public void AddListener( string eventName, EventCallback action )
    {
        DebugUtils.Assert(!string.IsNullOrEmpty(eventName), "Null or empty event name given to EventManager::AddListener");
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

    public void RemoveListener( string eventName, EventCallback action )
    {
        _callbacksToRemoveAtEndOfFrame.Add(new DeletePair() { eventName = eventName, callback = action });
    }

    public void SendEvent( string eventName, object param )
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

        CleanupRemovedCallbacks();
    }

    public Dictionary<string, int> GetListenerStats()
    {
#if UNITY_EDITOR
        Dictionary<string, int> stats = new Dictionary<string, int>();

        foreach( KeyValuePair< string, List<EventCallback> > pair in _listenersByEventName )
        {
            int count = 0;

            if( pair.Value != null )
            {
                count = pair.Value.Count;
            }

            stats[pair.Key] = count;
        }

        return stats;
#else
        return null;
#endif
    }
}
