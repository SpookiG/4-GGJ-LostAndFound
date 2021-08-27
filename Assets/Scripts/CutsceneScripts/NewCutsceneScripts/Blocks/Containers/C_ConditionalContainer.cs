using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Containers/Conditional Container")]
public class C_ConditionalContainer : C_CutsceneBlock
{
    public bool SkipIfNoneMet;
    public bool Async;
    public bool PersistantChecking;
    public ConditionToBlock[] ConditionsToBlocks;

    private bool _start;
    private List<C_CutsceneBlock> _blocks;
    private Dictionary<C_CutsceneBlock, CutsceneExitStatus> _fullProgress;





    public override void Init()
    {
        _start = true;
        foreach (ConditionToBlock c2b in ConditionsToBlocks)
        {
            c2b.Block.Init();
        }

        _fullProgress = new Dictionary<C_CutsceneBlock, CutsceneExitStatus>();
        foreach (ConditionToBlock c2b in ConditionsToBlocks)
        {
            _fullProgress[c2b.Block] = CutsceneExitStatus.running;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _start = true;
        _blocks = new List<C_CutsceneBlock>();

        _fullProgress = new Dictionary<C_CutsceneBlock, CutsceneExitStatus>();
        foreach (ConditionToBlock c2b in ConditionsToBlocks)
        {
            _fullProgress[c2b.Block] = CutsceneExitStatus.running;
        }
    }


    protected override CutsceneExitStatus _Run()
    {
        if (_start || PersistantChecking)
        {
            Check();
        }

        if (_blocks.Count == 0 && SkipIfNoneMet)
        {
            return CutsceneExitStatus.finished;
        }


        HashSet<CutsceneExitStatus> contentsStatuses = new HashSet<CutsceneExitStatus>();

        foreach (C_CutsceneBlock block in _blocks)
        {
            if (_fullProgress[block] != CutsceneExitStatus.finished)
            {
                _fullProgress[block] = block.Run();
            }

            contentsStatuses.Add(_fullProgress[block]);
        }


        /*for (int i = 0; i < _blocks.Count; i++)
        {
            if (_fullProgress[i] != CutsceneExitStatus.finished)
            {
                _fullProgress[i] = _blocks[i].Run();
            }
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


    private void Check()
    {
        _blocks.Clear();

        foreach (ConditionToBlock c2b in ConditionsToBlocks)
        {
            if (((C_Condition) c2b.Condition).Check)
            {
                _blocks.Add(c2b.Block);

                if (!Async)
                {
                    return;
                }
            }
        }
    }
}

[Serializable]
public class ConditionToBlock
{
    public C_Condition Condition;
    public C_CutsceneBlock Block;
}