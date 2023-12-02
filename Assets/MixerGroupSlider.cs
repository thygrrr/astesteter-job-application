using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[DisallowMultipleComponent]
public class MixerGroupSlider : MonoBehaviour
{
    public AudioMixerGroup group;

    [Tooltip("The minimum decibel value of the slider")]
    public float minDB = -80;

    [Tooltip("The maximum decibel value of the slider")]
    public float maxDB = 3;

    [Tooltip("Additionally scale the slider along a nonlinear curve to give more precision in the middle to top range.")]
    [Range(0.1f, 1f)]
    public float linearity = 0.5f;

    private Slider _slider;

    private void OnEnable()
    {
        LoadValue();
        _slider.onValueChanged.AddListener(OnValueChanged);
    }
    
    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void Start()
    {
        OnValueChanged(_slider.value);
    }

    private void LoadValue()
    {
        group.audioMixer.GetFloat(group.name, out var decibels);
        var normalized = math.saturate(math.remap(minDB, maxDB, 0, 1, decibels));
        var nonlinear = math.pow(normalized, 1f / linearity);
        var exponential = math.pow(10f, nonlinear);
        var remapped = math.remap(1, 10, 0, 1, exponential);
        _slider.value = remapped;
    }
    
    private void OnValueChanged(float sliderValue)
    {
        var remapped = math.remap(0, 1, 1, 10, sliderValue);
        var logarithmic = math.saturate(math.log10(remapped));
        var nonlinear = math.pow(logarithmic, linearity);
        var decibels = math.remap(0f, 1f, minDB, maxDB, nonlinear);
        group.audioMixer.SetFloat(group.name, decibels);
    }

    private void OnValidate()
    {
        if (!_slider) _slider = GetComponent<Slider>();
        _slider.minValue = 0;
        _slider.maxValue = 1;
    }
}
