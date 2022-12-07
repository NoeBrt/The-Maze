using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioSource> sfxSounds;
    private float sfxVolume;
    private float musicVolume;
    public List<AudioSource> SfxSounds { get => sfxSounds; set => sfxSounds = value; }
    public AudioSource MusicSource { get => musicSource; set => musicSource = value; }
    public float SfxVolume { get => sfxVolume; }
    public float MusicVolume { get => musicVolume; }
    public static SettingManager Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            FadeSoundValue();

        }
    }

    public void SaveInFile()
    {
        JsonSaveDAO jsonSaveDao = new JsonSaveDAO(Application.persistentDataPath);
        jsonSaveDao.updateGraphicQuality(QualitySettings.GetQualityLevel());
        jsonSaveDao.updateMusicVolume(MusicVolume);
        jsonSaveDao.updateSfxVolume(SfxVolume);
    }
    public void initSettingValue()
    {
        JsonSaveDAO jsonSaveDao = new JsonSaveDAO(Application.persistentDataPath);
        setMusicVolumeValue(jsonSaveDao.getMusicVolumeFromJson());
        setSfxVolumeValue(jsonSaveDao.getSfxVolumeFromJson());
        setGraphicsQuality(jsonSaveDao.getGraphicQualityFromJson());
    }
    public void FadeSoundValue() //weird code to do a fade
    {
        musicSource.volume = 0f;
        SfxSounds.ForEach(source => source.volume = 0f);
        JsonSaveDAO jsonSaveDao = new JsonSaveDAO(Application.persistentDataPath);
        setGraphicsQuality(jsonSaveDao.getGraphicQualityFromJson());
        musicVolume = jsonSaveDao.getMusicVolumeFromJson();
        sfxVolume = jsonSaveDao.getSfxVolumeFromJson();
        SoundFade.FadeOut(musicVolume, musicSource, Instance);
        foreach (AudioSource source in SfxSounds)
        {
            SoundFade.FadeOut(sfxVolume, source, Instance);
        }
    }
    public void setMusicVolumeValue(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
        if (MusicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    public void setSfxVolumeValue(float newSfxVolume)
    {
        sfxVolume = newSfxVolume;
        if (sfxSounds.Count > 0)
        {
            sfxSounds.ForEach(sfx => sfx.volume = sfxVolume);
        }
    }
    public void setGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
