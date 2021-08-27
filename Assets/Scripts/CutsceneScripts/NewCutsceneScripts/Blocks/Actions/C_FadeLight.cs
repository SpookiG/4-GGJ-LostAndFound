using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Fade Light")]
public class C_FadeLight : C_CutsceneBlock
{
    public float Speed;

    public float IntensityFrom;
    public float IntensityTo;

    public float RangeFrom;
    public float RangeTo;

    public Light ApplyTo;

    private TimeLerp<float> _intensityLerp;
    private TimeLerp<float> _rangeLerp;

    private void Start()
    {
        _intensityLerp = new TimeLerp<float>();
        _rangeLerp = new TimeLerp<float>();

        Init();
    }


    public override void Init()
    {
        _intensityLerp.Prep(IntensityFrom, IntensityTo, Speed);
        _rangeLerp.Prep(RangeFrom, RangeTo, Speed);
    }


    protected override CutsceneExitStatus _Run()
    {
        var intensity = _intensityLerp.Go();
        var range = _rangeLerp.Go();

        ApplyTo.intensity = intensity.position;
        ApplyTo.range = range.position;

        if (intensity.progress < 1)
        {
            return CutsceneExitStatus.running;
        }

        return CutsceneExitStatus.finished;
    }
}
