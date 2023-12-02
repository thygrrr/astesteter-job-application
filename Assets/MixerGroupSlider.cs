using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MixerGroupSlider : MonoBehaviour
{
    public AudioMixerGroup group;

    [Tooltip("The minimum decibel value of the slider")]
    public float minDB = -80;
    
    [Tooltip("The maximum decibel value of the slider")]
    public float maxDB = 7;

    [Range(0.5f, 3f)]
    [Tooltip("Slider curve weighting on top of logarithmic, 1 = linear, 2 = quadratic, 3 = cubic, etc.")]
    public float exponent = 2;
    

    private Slider _slider;

    private void Awake()
    {
        if (!_slider) _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        LoadValue();
        _slider.onValueChanged.AddListener(OnValueChanged);
    }
    
    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void LoadValue()
    {
        group.audioMixer.GetFloat(group.name, out var decibels);
        var normalized = math.saturate(math.remap(minDB, maxDB, 0, 1, decibels));
        var power = math.pow(normalized, exponent);
        _slider.value = math.pow(10, power);
    }
    
    private void OnValueChanged(float sliderValue)
    {
        var root = math.pow(math.log10(sliderValue), 1.0f/exponent);
        var decibels = math.remap(0f, 1f, minDB, maxDB, root);
        group.audioMixer.SetFloat(group.name, decibels);
    }

    private void OnValidate()
    {
        if (!_slider) _slider = GetComponent<Slider>();
        _slider.minValue = 1;
        _slider.maxValue = 10;
    }
}
