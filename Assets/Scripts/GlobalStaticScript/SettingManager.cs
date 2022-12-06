using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioSource> sfxSounds;

    public int GraphicQualityIndex { get; set; }
    public float sfxVolume { get; set; }
    public float musicVolume { get; set; }
    public List<AudioSource> SfxSounds { get => sfxSounds; set => sfxSounds = value; }
    public AudioSource MusicSource { get => musicSource; set => musicSource = value; }
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
            initSettingValue();
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }

    public void SaveInFile()
    {
        JsonSaveDAO jsonSaveDao = new JsonSaveDAO(Application.persistentDataPath);
        jsonSaveDao.updateGraphicQuality(GraphicQualityIndex);
        jsonSaveDao.updateMusicVolume(musicVolume);
        jsonSaveDao.updateSfxVolume(sfxVolume);
    }
    void initSettingValue()
    {
        JsonSaveDAO jsonSaveDao = new JsonSaveDAO(Application.persistentDataPath);
        setMusicVolumeValue(jsonSaveDao.getMusicVolumeFromJson());
        setSfxVolumeValue(jsonSaveDao.getSfxVolumeFromJson());
        GraphicQualityIndex = jsonSaveDao.getGraphicQualityFromJson();
    }
    void setMusicVolumeValue(float musicVolume1)
    {
        musicVolume = musicVolume1;
        musicSource.volume = musicVolume;
    }
    void setSfxVolumeValue(float sfxVolume1)
    {
        sfxVolume = sfxVolume1;
        if (sfxSounds.Count > 0)
        {
            sfxSounds.ForEach(sfx => sfx.volume = sfxVolume);
        }
    }


}
