using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Maze : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector3 nodeScale;
    [SerializeField] Material finishMaterial;
    [SerializeField] Material startMaterial;
    [SerializeField] GameObject plane;
    [SerializeField] public AudioClip endWallSound;
    public bool IsFinished { get; set; } = false;
    public List<MazeNode> Nodes { get; set; }

    public MazeNode StartNode { get; set; }
    public MazeNode FinishNode { get; set; }
    public Vector2Int Size { get => size; set => size = value; }
    public Vector3 NodeScale { get => nodeScale; set => nodeScale = value; }

    public Material StartMaterial { get => startMaterial; set => startMaterial = value; }
    public GameObject Plane { get => plane; set => plane = value; }
    public Material FinishMaterial { get => finishMaterial; set => finishMaterial = value; }


    public void setNodesVisibility(bool isNodesVisible)
    {
        foreach (MazeNode m in Nodes)
        {
            m.gameObject.SetActive(true);
        }

    }
    public void setWallsMaterial(Material m)
    {
        GetComponent<MeshRenderer>().material = m;
        if (FinishNode == null)
            return;
        foreach (GameObject wall in FinishNode.Walls)
        {
            if (wall != null && wall.CompareTag("FinishWall"))
            {
                wall.GetComponent<MeshRenderer>().material = m;
            }

        }
    }

    public void setFinishWallsSound()
    {
        if (FinishNode != null)
        {
            AudioSource WallSource = FinishNode.Walls[0].AddComponent<AudioSource>();
            WallSource.clip = endWallSound;
            WallSource.spatialize = true;
            WallSource.spatializePostEffects = true;
            WallSource.dopplerLevel = 0;
            WallSource.spatialBlend = 1;
            SteamAudio.SteamAudioSource WallSteamAudioSource = FinishNode.Walls[0].AddComponent<SteamAudio.SteamAudioSource>();
            WallSteamAudioSource.directBinaural = true;
            WallSteamAudioSource.directivity = true;
            WallSteamAudioSource.occlusion = true;
            WallSteamAudioSource.distanceAttenuation = true;
            WallSteamAudioSource.useDistanceCurveForReflections = false;
            WallSource.volume = SettingManager.Instance.SfxVolume;
            SettingManager.Instance.SfxSounds.Add(WallSource);
            WallSource.Play();
        }
    }
}




// Update is called once per frame
