using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Containers/Async Container")]
public class C_AsyncContainer : C_CutsceneBlock//, C_IContainer
{
    //[HideInInspector] public bool Trigger { set; private get; }

    private List<C_CutsceneBlock> _blocks;
    private CutsceneExitStatus[] _fullProgress;


    public override void Init()
    {
        _fullProgress = new CutsceneExitStatus[_blocks.Count];
        foreach (C_CutsceneBlock block in _blocks)
        {
            block.Init();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _blocks = new List<C_CutsceneBlock>();

        foreach (Transform child in transform)
        {
            foreach (C_CutsceneBlock block in child.GetComponents<C_CutsceneBlock>())
            {
                _blocks.Add(block);
            }
        }

        _fullProgress = new CutsceneExitStatus[_blocks.Count];

        //Debug.Log(name + ": " + _fullProgress.Length);
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
        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_fullProgress[i] != CutsceneExitStatus.finished)
            {
                _fullProgress[i] = _blocks[i].Run();
            }
        }

        //Debug.Log(_fullProgress.Length);

        HashSet<CutsceneExitStatus> contentsStatuses = new HashSet<CutsceneExitStatus>(_fullProgress);

        /*foreach (CutsceneExitStatus stat in contentsStatuses)
        {
            Debug.Log(stat);
        }*/

        if (contentsStatuses.Contains(CutsceneExitStatus.endCutscene))
        {
            return CutsceneExitStatus.endCutscene;
        }

        if (contentsStatuses.Contains(CutsceneExitStatus.paused))
        {
            return CutsceneExitStatus.paused;
        }

        if (contentsStatuses.Contains(CutsceneExitStatus.running))
        {
            return CutsceneExitStatus.running;
        }

        return CutsceneExitStatus.finished;
    }
}
