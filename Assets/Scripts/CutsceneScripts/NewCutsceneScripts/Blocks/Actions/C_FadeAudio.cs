using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Fade Audio")]
public class C_FadeAudio : C_CutsceneBlock
{
    public float Speed;

    public float VolumeFrom;
    public float VolumeTo;

    public AudioSource ApplyTo;

    private TimeLerp<float> _volumeLerp;

    private void Start()
    {
        _volumeLerp = new TimeLerp<float>();

        Init();
    }


    public override void Init()
    {
        _volumeLerp.Prep(VolumeFrom, VolumeTo, Speed);
    }


    protected override CutsceneExitStatus _Run()
    {
        var volume = _volumeLerp.Go();

        ApplyTo.volume = volume.position;

        if (volume.progress < 1)
        {
            return CutsceneExitStatus.running;
        }

        return CutsceneExitStatus.finished;
    }
}
