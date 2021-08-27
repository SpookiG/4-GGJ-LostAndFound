using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Particle Switch")]
public class C_ParticleSwitch : C_CutsceneBlock
{
    public bool Switch;
    public ParticleSystem Particles;

    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        if (Switch)
        {
            Particles.Clear();
            Particles.Play();
        }
        else
        {
            Particles.Stop();
        }

        return CutsceneExitStatus.finished;
    }
}
