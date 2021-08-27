using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// looks like I might not have needed this at all oops, maybe delete later??
[AddComponentMenu("Cutscene/Blocks/Actions/Wait For (May delete later)")]
public class C_WaitFor : C_CutsceneBlock
{
    private static Dictionary<string, List<bool>> _things = new Dictionary<string, List<bool>>();

    public string ThingName;

    private int? _thingIndex = null;

    // Start is called before the first frame update
    void Start()
    {
        _things[ThingName] = _things.ContainsKey(ThingName) ? _things[ThingName] : new List<bool>();

        if (_thingIndex == null)
        {
            _thingIndex = _things[ThingName].Count;
            _things[ThingName].Add(false);
        }

    }



    public override void Init()
    {
        _things[ThingName][(int)_thingIndex] = false;
    }

    protected override CutsceneExitStatus _Run()
    {
        _things[ThingName][(int)_thingIndex] = true;

        if (_things[ThingName].Aggregate(true, (acc, x) => acc && x))
        {
            return CutsceneExitStatus.finished;
        }

        return CutsceneExitStatus.running;
    }
}
