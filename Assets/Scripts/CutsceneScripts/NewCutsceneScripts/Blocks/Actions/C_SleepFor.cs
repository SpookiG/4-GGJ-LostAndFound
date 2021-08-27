using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Sleep For")]
public class C_SleepFor : C_CutsceneBlock
{
    [Header("Maximum Wait Time is 4294967295 Milliseconds")]
    public uint SleepTime;

    private uint _sleptFor;


    void Start()
    {
        _sleptFor = 0;
    }


    public override void Init()
    {
        _sleptFor = 0;
    }

    protected override CutsceneExitStatus _Run()
    {
        uint updatedSleptFor = _sleptFor + (uint) Mathf.RoundToInt(Time.deltaTime * 1000);
        _sleptFor = updatedSleptFor >= _sleptFor ? updatedSleptFor : uint.MaxValue;             // uint overflow check

        if (_sleptFor < SleepTime)
        {
            return CutsceneExitStatus.running;
        }

        return CutsceneExitStatus.finished;
    }
}
