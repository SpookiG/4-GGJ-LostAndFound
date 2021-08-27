using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Triggers/Trigger From Cutscene")]
public class C_TriggerFromCutscene : C_CutsceneBlock
{
    public C_RunnerBean Bean;

    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        Bean.enabled = true;

        return CutsceneExitStatus.finished;
    }
}
