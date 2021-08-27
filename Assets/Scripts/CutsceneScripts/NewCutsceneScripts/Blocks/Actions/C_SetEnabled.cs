using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Set Enabled")]
public class C_SetEnabled : C_CutsceneBlock
{
    public bool SetEnabledTo;
    public Behaviour ApplyTo;

    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        ApplyTo.enabled = SetEnabledTo;

        return CutsceneExitStatus.finished;
    }
}
