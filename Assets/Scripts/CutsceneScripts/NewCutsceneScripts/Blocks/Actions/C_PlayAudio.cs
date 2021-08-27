using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Play Audio")]
public class C_PlayAudio : C_CutsceneBlock
{
    public AudioSource AudioSource;
    private bool _start;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _start = true;
    }


    protected override CutsceneExitStatus _Run()
    {
        if (_start)
        {
            AudioSource.Play();
            _start = false;
        }

        if (AudioSource.isPlaying)
        {
            return CutsceneExitStatus.running;
        }

        return CutsceneExitStatus.finished;
    }
}
