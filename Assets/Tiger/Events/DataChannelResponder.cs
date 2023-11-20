using Tiger.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Tiger.Events
{
    public abstract class DataChannelResponder<TChannel, T> : SealableLifecycleBehaviour where TChannel : DataChannel<T>
    {
        [SerializeField]
        protected TChannel channel;

        [SerializeField]
        protected UnityEvent<T> action;

        protected sealed override void Awake()
        {
            if (channel) channel.subscribers.AddListener(Trigger);
        }

        protected sealed override void OnDestroy()
        {
            if (channel) channel.subscribers.RemoveListener(Trigger);
        }

        protected virtual void OnEvent(T data)
        {
            //Implement this if you want to have your own code happen.
            //TODO: Maybe make a separate extensible abstract class.
        }
        
        // Triggered when the event(s) happen.
        private void Trigger(T data)
        {
            OnEvent(data);
            action.Invoke(data);
        }
        
#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (channel)
            {
                Handles.Label(Handles.matrix * transform.position, $"→{channel.name}");
            }
            else
            {
                Handles.Label(transform.position, "not subscribed");
            }
        }

        protected virtual void OnValidate()
        {
            if (!channel) Debug.Log("DataChannelResponder: Channel is not set.", this);
        }
#endif
    }
}

/*
Written by Tiger Blue in 2021

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/