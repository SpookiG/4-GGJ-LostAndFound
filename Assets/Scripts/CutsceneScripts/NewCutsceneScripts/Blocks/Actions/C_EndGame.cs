using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/End Game")]
public class C_EndGame : C_CutsceneBlock
{
    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        Application.Quit();
        Debug.Log("Game ended");
        Debug.Log("Game ended");

        return CutsceneExitStatus.endCutscene;
    }
}
