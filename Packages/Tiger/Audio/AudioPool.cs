using System.Collections.Generic;
using Tiger.Util;
using UnityEngine;

namespace Tiger.Audio
{
    /// <summary>
    /// A basic component that creates audio sources on an object and serves as a point to access them.
    /// </summary>
    public class AudioPool : MonoBehaviour
    {
        [SerializeField]
        private int capacity = 5;
        
        internal readonly List<AudioSource> sources = new();
        
        private void Awake()
        {
            Populate();
        }

        private void Populate()
        {
            //Add missing sources.
            for (var i = sources.Count; i < capacity; i++) sources.Add(gameObject.AddComponent<AudioSource>());
        }

        /// <summary>
        /// Provides an AudioSource from the pool for purposes of holding it for a longer time for later use.
        /// Example use case: playing a sound on a loop, but then stopping it later.
        /// </summary>
        /// <returns>The source</returns>
        public AudioSource Take()
        {
            Populate();
            return sources.Take();   
        }
    }
}
