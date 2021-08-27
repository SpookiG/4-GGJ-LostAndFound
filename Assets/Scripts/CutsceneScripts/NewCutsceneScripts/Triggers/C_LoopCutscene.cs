using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("Cutscene/Triggers/Loop Cutscene")]
public class C_LoopCutscene : MonoBehaviour
{
    public C_RunnerBean Bean;

    // Start is called before the first frame update
    void Start()
    {
        Bean = Bean != null ? Bean : GetComponent<C_RunnerBean>();
    }

    // Update is called once per frame
    void Update()
    {
        Bean.enabled = true;
    }
}
