using System.Collections;
using System.Linq;

using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class optionUi : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] ToggleGroup toggleGroup;
    void Awake()
    {
        musicSlider.value = SettingManager.Instance.MusicVolume;
        sfxSlider.value = SettingManager.Instance.SfxVolume;
        toggleGroup.GetComponentsInChildren<Toggle>()[QualitySettings.GetQualityLevel()].isOn = true;
    }
    private void OnEnable()
    {
        musicSlider.value = SettingManager.Instance.MusicVolume;
        sfxSlider.value = SettingManager.Instance.SfxVolume;
        toggleGroup.GetComponentsInChildren<Toggle>()[QualitySettings.GetQualityLevel()].isOn = true;
    }

}
