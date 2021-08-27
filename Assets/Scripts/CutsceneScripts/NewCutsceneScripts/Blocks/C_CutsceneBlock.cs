using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class C_CutsceneBlock : MonoBehaviour
{
    public CutsceneExitStatus BlockStatus;

    public CutsceneExitStatus Run()
    {
        if (BlockStatus != CutsceneExitStatus.running)
        {
            Init();
            return BlockStatus;
        }


        CutsceneExitStatus exitStatus = _Run();

        if (exitStatus == CutsceneExitStatus.finished || exitStatus == CutsceneExitStatus.endCutscene)
        {
            Init();
        }

        return exitStatus;
    }

    // implement these two only, not the above
    public abstract void Init(); //public?
    protected abstract CutsceneExitStatus _Run();
}

// what does selecting each of these in the inspector do?
public enum CutsceneExitStatus
{
    running,            // run as normal
    finished,           // skip only this block
    paused,             // halt on this block until status is changed (programatically)
    endCutscene         // skip any remaining parts of the cutscene
}

