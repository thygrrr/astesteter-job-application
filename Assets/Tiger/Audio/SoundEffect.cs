//SPDX-License-Identifier: Unlicense

using System.Collections.Generic;
using Tiger.Attributes;
using Tiger.Util;
using UnityEngine;
using UnityEngine.Audio;

namespace Tiger.Audio
{
    [CreateAssetMenu(menuName = "Sound/Sound Effect")]
    [Icon("Assets/Tiger/Audio/Editor/sound.png")]
    public class SoundEffect : AudioEvent
    {
        [Header("Audio Palette")]
        public List<AudioClip> clips;

        [Header("Mixer")] 
        public AudioMixerGroup group;

        [Header("Variation Settings")]
        [MinMaxRange(0, 3)]
        public RangedFloat delay;

        public RangedFloat volume = new() {minimum = 0.9f, maximum = 1.0f};
        
        [MinMaxRange(0, 2)]
        public RangedFloat pitch = new() {minimum = 0.9f, maximum = 1.1f};

        public bool loop;
        
        public override void Play(AudioSource source)
        {
            source.clip = clips.Pick();
            source.pitch = pitch.Random();
            source.volume = volume.Random();
            source.outputAudioMixerGroup = group;
            source.PlayDelayed(delay.Random());
        }
    }    
}


/*
Written by Moritz Voss in 2018, 2023

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