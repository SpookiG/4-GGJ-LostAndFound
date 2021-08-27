using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Cutscene/Blocks/Actions/Save And End Game")]
public class SaveAndEndGame : C_CutsceneBlock
{
    public override void Init()
    {

    }


    protected override CutsceneExitStatus _Run()
    {
        PlayerPrefs.SetInt(SaveDataLookup.GameComplete, ToInt(true));

        Application.Quit();
        Debug.Log("Game ended for good");

        return CutsceneExitStatus.endCutscene;
    }


    private int ToInt(bool b)
    {
        // 0 is false in C
        return b == false ? 0 : 1;
    }
}
