using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SliderSpeed : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TMP_Text valueLabel;

    [SerializeField]
    private string prefsKey = "PlayerMoveSpeed";

    [SerializeField]
    private float defaultSpeed = 6f;

    private void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
        float saved = PlayerPrefs.GetFloat(prefsKey, defaultSpeed);
        slider.value = Mathf.Clamp(saved, slider.minValue, slider.maxValue);
        UpdateLabel(slider.value);
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDestroy() => slider.onValueChanged.RemoveListener(OnSliderChanged);

    private void OnSliderChanged(float value)
    {
        UpdateLabel(value);
        PlayerPrefs.SetFloat(prefsKey, value);
        PlayerPrefs.Save();
    }

    private void UpdateLabel(float v)
    {
        if (valueLabel != null) valueLabel.text = $"{v:0.0}";
    }
}