using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Conditions/Condition From Cutscene")]
public class C_ConditionFromCutscene : C_Condition
{
    public bool SetTo;

    protected override CutsceneExitStatus _Run()
    {
        Check = SetTo;

        return CutsceneExitStatus.finished;
    }
}
