using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Set Block Status")]
public class C_SetBlockStatus : C_CutsceneBlock
{
    public CutsceneExitStatus SetStatusTo;
    public C_CutsceneBlock ApplyTo;

    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        ApplyTo.BlockStatus = SetStatusTo;

        return CutsceneExitStatus.finished;
    }
}
