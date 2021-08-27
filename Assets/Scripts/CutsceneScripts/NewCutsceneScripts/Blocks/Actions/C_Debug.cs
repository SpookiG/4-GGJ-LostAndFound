using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Debug")]
public class C_Debug : C_CutsceneBlock
{
    public string DebugMessage;


    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        Debug.Log(DebugMessage);

        return CutsceneExitStatus.finished;
    }
}
