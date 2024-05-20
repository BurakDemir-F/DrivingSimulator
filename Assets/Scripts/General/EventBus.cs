using System;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public abstract class EventBus<T> : MonoBehaviour where T : System.Enum
    {
        private Dictionary<T,HashSet<Action<EventBusData>>> _subscriberActions;
        private HashSet<Action<T, EventBusData>> _actionSet;
        private event Action<T, EventBusData> _action;

        public EventBus()
        {
            _actionSet = new HashSet<Action<T, EventBusData>>();
            _subscriberActions = new Dictionary<T, HashSet<Action<EventBusData>>>();
        }

        public void Subscribe(T subscriptionType, Action<EventBusData> function)
        {
            if (_subscriberActions.TryGetValue(subscriptionType, out var actionSet))
            {
                if (actionSet.Contains(function))
                    return;
                
                actionSet.Add(function);
            }
            else
            {
                _subscriberActions.Add(subscriptionType,new HashSet<Action<EventBusData>>(){function});
            }
        }

        public void UnSubscribe(T subscriptionType, Action<EventBusData> function)
        {
            if(!_subscriberActions.TryGetValue(subscriptionType,out var actionSet))
                return;
            
            if(!actionSet.Contains(function))
                return;

            actionSet.Remove(function);
        }

        public void Subscribe(Action<T,EventBusData> function)
        {
            if (_actionSet.Contains(function))
                return;

            _actionSet.Add(function);
            _action += function;
        }

        public void UnSubscribe(Action<T,EventBusData> function)
        {
            if (!_actionSet.Contains(function))
                return;

            _actionSet.Remove(function);
            _action -= function;
        }

        public void Publish(T parameterEnum,EventBusData eventBusData)
        {
            if (_subscriberActions.TryGetValue(parameterEnum, out var actions))
            {
                foreach (var action in actions)
                {
                    action?.Invoke(eventBusData);
                }
            }
            
            _action?.Invoke(parameterEnum,eventBusData);
        }
    }

    public class EventBusData
    {
        
    }
}