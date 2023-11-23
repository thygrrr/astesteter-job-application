using System.Collections.Generic;
using Tiger.Util;
using UnityEngine;

namespace Tiger.Audio
{
    /// <inheritdoc />
    /// <summary>
    /// Contains the abstract capability of being a playable audio asset for a given AudioSource 
    /// </summary>
    [Icon("Assets/Tiger/Audio/Editor/Icons/sound.png")]
    public abstract class AudioEvent : ScriptableObject
    {
        /// <summary>
        /// Play this Audio.
        /// </summary>
        /// <param name="source">the AudioSource to play it on.</param>
        public abstract void Play(AudioSource source);

        /// <summary>
        /// Play this Audio on the given ring buffer (IList) of audio sources.
        /// It will play it on the first one, then move that source to the end of the list.
        /// </summary>
        /// <param name="sourcesRingBuffer">the AudioSources to shift and then play it on.</param>
        public void Play(IList<AudioSource> sourcesRingBuffer) => Play(sourcesRingBuffer.Shift());
    }
}

/*
Written by Moritz Voss in 2018

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