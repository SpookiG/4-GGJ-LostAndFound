using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class C_Condition : C_CutsceneBlock
{
    public bool Check { get; set; }

    public override void Init()
    {

    }

    protected override CutsceneExitStatus _Run()
    {
        return CutsceneExitStatus.finished;
    }
}
