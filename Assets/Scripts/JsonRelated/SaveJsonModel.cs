using System.Collections;
using System.Collections.Generic;
using System;
public class SaveJsonModel
{

    public float musicVolume;
    public float sfxVolume;
    public int graphicQuality;

    public SaveJsonModel(float musicVolume, float sfxVolume, int GraphicQuality)
    {
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.graphicQuality = GraphicQuality;
    }
    public SaveJsonModel()
    {
        this.musicVolume = 0.7f;
        this.sfxVolume = 0.7f;
        this.graphicQuality = 5;
    }


    public override string ToString()
    {
        return base.ToString() + " musicVolume=" + musicVolume + ", sfxVolume=" + sfxVolume + ", Graphic Quality= " + graphicQuality;
    }
}