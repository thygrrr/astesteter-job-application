// SPDX-License-Identifier: Unlicense

using UnityEngine;
using Random = UnityEngine.Random;

namespace Tiger.ScreenShake
{
    public class ScreenShakeResponder : MonoBehaviour
    {
        [Header("Object to Shake (e.g. Camera)")]
        public Transform target;

        [Header("Observer to perceive trauma from (e.g. Player)")]
        public Transform perceiver;

        [Header("Trauma Parameters")]
        [Range(0f, 1f)] public float highFrequencyAttack = 0.05f;
        [Range(0f, 2f)] public float highFrequencySustain = 0.5f;

        [Range(0f, 1f)] public float lowFrequencyAttack = 0.2f;
        [Range(0f, 2f)] public float lowFrequencySustain = 0.8f;

        [Range(0.1f, 5f)] [Tooltip("Shape of the curve, trauma values are from 0..1 and are raised to this power.")]
        public float traumaHFExponent = 2f;

        [Range(0.1f, 5f)] [Tooltip("Shape of the curve, trauma values are from 0..1 and are raised to this power.")]
        public float traumaLFExponent = 2f;

        [Range(0.1f, 5f)] public float highFrequencyDistanceFallOff = 2f;
        [Range(0.1f, 5f)] public float lowFrequencyDistanceFallOff = 1f;

        [Range(1f, 50f)] public float highFrequency = 30.0f;
        [Range(1f, 50f)] public float lowFrequency = 5.0f;
        
        private float _traumaHF;
        private float _traumaLF;

        private float _traumaHFAttackDerivative;
        private float _traumaLFAttackDerivative;

        private float _traumaHFSustainDerivative;
        private float _traumaLFSustainDerivative;

        private float _traumaHFsmooth;
        private float _traumaLFsmooth;

        public Vector3 shakeAxisStrengthsHigh = Vector3.one;
        public Vector3 shakeAxisStrengthsLow = Vector3.one;

        private Vector3 _noiseIndex;

        public ShakeType shakeType;

        public enum ShakeType
        {
            Rotational,
            Positional
        }

        private void OnEnable()
        {
            if (!perceiver) perceiver = target;
            _noiseIndex = Random.onUnitSphere * 10f;
            ScreenShake.Shakes.AddListener(OnShake);
        }


        private void OnDisable()
        {
            ScreenShake.Shakes.RemoveListener(OnShake);
        }


        [ContextMenu("Test Shake Both")]
        public void TestShakeBoth()
        {
            ScreenShake.Add(Vector3.zero, 5, 5);
        }

        [ContextMenu("Test Shake Low")]
        public void TestShakeLow()
        {
            ScreenShake.Add(Vector3.zero, 0, 5);
        }

        [ContextMenu("Test Shake High")]
        public void TestShakeHigh()
        {
            ScreenShake.Add(Vector3.zero, 5, 0);
        }

        private void OnShake(ScreenShake.ShakeEvent shake)
        {
            var distance = Mathf.Max(1, (perceiver.position - shake.position).magnitude);

            _traumaHF += shake.amplitudeHF / Mathf.Pow(distance, highFrequencyDistanceFallOff);
            _traumaHF = Mathf.Clamp01(_traumaHF);

            _traumaLF += shake.amplitudeLF / Mathf.Pow(distance, lowFrequencyDistanceFallOff);
            _traumaLF = Mathf.Clamp01(_traumaLF);
        }

        private void Update()
        {
            _traumaHFsmooth = Mathf.SmoothDamp(_traumaHFsmooth, _traumaHF, ref _traumaHFAttackDerivative, highFrequencyAttack);
            _traumaLFsmooth = Mathf.SmoothDamp(_traumaLFsmooth, _traumaLF, ref _traumaLFAttackDerivative, lowFrequencyAttack);

            //Apply the actual shake.
            var effectiveHF = CalculateOffset(Mathf.Pow(_traumaHFsmooth, traumaHFExponent), highFrequency, shakeAxisStrengthsHigh);
            var effectiveLF = CalculateOffset(Mathf.Pow(_traumaLFsmooth, traumaLFExponent), lowFrequency, shakeAxisStrengthsLow);

            switch (shakeType)
            {
                case ShakeType.Rotational:
                    target.localRotation = Quaternion.Euler(effectiveHF + effectiveLF);
                    break;
                case ShakeType.Positional:
                    target.localPosition = effectiveHF + effectiveLF;
                    break;
            }

            _traumaHF = Mathf.SmoothDamp(_traumaHF, 0, ref _traumaHFSustainDerivative, highFrequencySustain);
            _traumaLF = Mathf.SmoothDamp(_traumaLF, 0, ref _traumaLFSustainDerivative, lowFrequencySustain);
        }

        private Vector3 CalculateOffset(float amplitude, float frequency, Vector3 strength)
        {
            return new Vector3()
            {
                x = strength.x * amplitude * (0.5f - Mathf.PerlinNoise(_noiseIndex.x, Time.time * frequency)),
                y = strength.y * amplitude * (0.5f - Mathf.PerlinNoise(_noiseIndex.y, Time.time * frequency)),
                z = strength.z * amplitude * (0.5f - Mathf.PerlinNoise(_noiseIndex.z, Time.time * frequency))
            };
        }
    }
}

/*
Written by Tiger Blue in 2017, 2023 (filters modernized, inspector modernized)

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