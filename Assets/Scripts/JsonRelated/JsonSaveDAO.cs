using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaveDAO
{
    public static string saveName = "GameInfo.json";
    string path;
    private SaveJsonModel model { get; }
    public JsonSaveDAO(string path)
    {
        model = new SaveJsonModel();
        path += "/GameInfo.json";
        this.path = path;
        if (!File.Exists(path) || getModelFromJson() == null)
        {
            File.Create(path).Close();
            setJsonFileModel(model);
        }
        else
        {
            model = getModelFromJson();
        }
    }
    private void setJsonFileModel(SaveJsonModel model)
    {
        File.WriteAllText(path, JsonUtility.ToJson(model));
    }

    public void updateMusicVolume(float value)
    {
        model.musicVolume = value;
        setJsonFileModel(model);
        Debug.Log(JsonUtility.ToJson(model));

    }
    public void updateSfxVolume(float value)
    {
        model.sfxVolume = value;
        setJsonFileModel(model);
        Debug.Log(JsonUtility.ToJson(model));

    }

    public void updateGraphicQuality(int value)
    {
        model.graphicQuality = value;
        setJsonFileModel(model);
        Debug.Log(JsonUtility.ToJson(model));

    }


    SaveJsonModel getModelFromJson()
    {
        return JsonUtility.FromJson<SaveJsonModel>(File.ReadAllText(path));
    }
    public int getGraphicQualityFromJson()
    {
        return JsonUtility.FromJson<SaveJsonModel>(File.ReadAllText(path)).graphicQuality;
    }
    public float getMusicVolumeFromJson()
    {
        return JsonUtility.FromJson<SaveJsonModel>(File.ReadAllText(path)).musicVolume;
    }
    public float getSfxVolumeFromJson()
    {
        return JsonUtility.FromJson<SaveJsonModel>(File.ReadAllText(path)).sfxVolume;
    }

}

