using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Runner Bean")]
public class C_RunnerBean : MonoBehaviour
{
    public C_CutsceneBlock CutsceneBlock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CutsceneExitStatus blockStatus = CutsceneBlock.Run();

        if (blockStatus == CutsceneExitStatus.finished || blockStatus == CutsceneExitStatus.endCutscene)
        {
            enabled = false;
        }
    }
}
