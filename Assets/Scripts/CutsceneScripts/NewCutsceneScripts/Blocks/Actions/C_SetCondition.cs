using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Set Condition")]
public class C_SetCondition : C_CutsceneBlock
{
    public bool SetTo;
    [SerializeField] public C_Condition[] Conditions;

    [HideInInspector] public bool Check { get; set; }

    public override void Init()
    {

    }

    protected override CutsceneExitStatus _Run()
    {
        foreach (C_Condition condition in Conditions)
        {
            condition.Check = SetTo;
        }

        return CutsceneExitStatus.finished;
    }
}
