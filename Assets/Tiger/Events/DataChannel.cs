//SPDX-License-Identifier: Unlicense

using System;
using System.Collections.Generic;
using System.IO;
using Loggers;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Tiger.Events
{
    [Icon("Assets/Tiger/Events/Editor/Icons/channel.png")]
    public class DataChannel<T> : ScriptableObject
    {
        [SerializeField] private DebugSettings debugSettings;

        [SerializeField] private DefaultValueBehaviour onValueReadBeforeFirstWrite;

        [NonSerialized]
        private readonly UnityEvent<T> _subscriptions = new();

        private T _value;

        /// <summary>
        /// The last value published on this Channel. Great for components that just care about the latest value once, and don't want to
        /// book keep the value constantly  through a subscription.
        /// </summary>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T value => !EqualityComparer<T>.Default.Equals(_value, default) ? _value : HandleUninitializedRead();

        /// <summary>
        /// Subscribe to this channel.
        /// </summary>
        /// <param name="action">callback that will be invoked on Emit.</param>
        public void Subscribe(UnityAction<T> action)
        {
            _subscriptions.AddListener(action);
        }

        /// <summary>
        /// Unsubscribe from this channel.
        /// </summary>
        /// <param name="action">the callback to remove</param>
        public void Unsubscribe(UnityAction<T> action)
        {
            _subscriptions.RemoveListener(action);
        }


        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Emit a value on this channel. All subscriber actions will be invoked with the value.
        /// </summary>
        /// <param name="data">An object or value of type T</param>
        /// <param name="context">UnityEngine.Object that become the potential Debut console highlight cuplrit.</param>
        public void Emit(T data, Object context = null)
        {
            if (debugSettings.enabled) debugSettings.Log($"<b>EVENT</b> {name} : {data}", context != null ? context : this);
            _value = data;
            _subscriptions.Invoke(data);
        }


    private T HandleUninitializedRead()
    {
        switch (onValueReadBeforeFirstWrite)
        {
            case DefaultValueBehaviour.ThrowException:
                throw new InvalidDataException($"DataChannel<{typeof(T).Name}> {name} accessed before first Emit()");

            case DefaultValueBehaviour.LogError:
                Debug.LogError($"DataChannel<{typeof(T).Name}> {name} accessed before first before first Emit()", this);
                return default;

            case DefaultValueBehaviour.LogWarning:
                Debug.LogError($"DataChannel<{typeof(T).Name}> {name} accessed before first before first Emit()", this);
                return default;

            case DefaultValueBehaviour.ReturnDefaultValueForType:
                return default;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    }
    
    internal enum DefaultValueBehaviour
    {
        ThrowException = default,
        LogError,
        LogWarning,
        ReturnDefaultValueForType,
    }
}

/*
Written by Tiger Blue in 2021, 2023

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