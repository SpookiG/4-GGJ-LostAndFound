using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public interface C_IContainer
{
    bool Trigger { set; }
}*/

[AddComponentMenu("Cutscene/Blocks/Containers/Stepwise Container")]
public class C_StepwiseContainer : C_CutsceneBlock//, C_IContainer
{
    //[HideInInspector] public bool Trigger { set; private get; }

    private int _index;
    private List<C_CutsceneBlock> _blocks;


    public override void Init()
    {
        _index = 0;
        foreach (C_CutsceneBlock block in _blocks)
        {
            block.Init();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _index = 0;
        _blocks = new List<C_CutsceneBlock>();

        foreach (Transform child in transform)
        {
            foreach (C_CutsceneBlock block in child.GetComponents<C_CutsceneBlock>())
            {
                _blocks.Add(block);
            }
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Trigger)
        {
            CutsceneExitStatus blockStatus = Run();

            if (blockStatus == CutsceneExitStatus.finished || blockStatus == CutsceneExitStatus.endCutscene)
            {
                Trigger = false;
            }
        }
    }*/


    protected override CutsceneExitStatus _Run()
    {
        if (_index >= _blocks.Count)
        {
            // TODO: check this can be changed via a Trigger script
            return CutsceneExitStatus.finished;
        }

        CutsceneExitStatus contentsStatus = _blocks[_index].Run();

        if (contentsStatus == CutsceneExitStatus.finished)
        {
            _index++;
            return _Run(); // depending on how long a chain of one frame changes takes, it may be better just to return CutsceneExitStatus.running. Check later for lag
        }

        /*// TODO: Init func to replace this hack
        if (contentsStatus == CutsceneExitStatus.endCutscene)
        {
            _index = 0;
        }*/

        return contentsStatus;
    }
}