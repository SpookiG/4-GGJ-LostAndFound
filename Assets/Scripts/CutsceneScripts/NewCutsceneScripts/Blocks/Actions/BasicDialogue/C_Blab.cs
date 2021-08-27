using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Basic Dialogue/Blab")]
public class C_Blab : C_BasicDialogue
{
    public float RambleTimeStep = 0.02f;
    public float RushTimeStep = 0.002f;

    private float _timeSinceLastBlab;


    public override void Init()
    {
        base.Init();
        _dialogueIndex = 0;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        _timeSinceLastBlab = 0;
    }

    protected override CutsceneExitStatus _Run()
    {
        CutsceneExitStatus finished = base._Run();
        _timeSinceLastBlab += Time.deltaTime;

        switch (Panel.Mode)
        {
            case DialogueMode.ramble:
                _panelAudioSource.pitch = 1f;
                if (!_panelAudioSource.isPlaying)
                {
                    _panelAudioSource.clip = SpeakerInfo.BlabNoises[0];
                    _panelAudioSource.Play();
                }

                // if not a stopping point, append next letter
                if (_timeSinceLastBlab >= RambleTimeStep)
                {

                    _textMesh.text += _dialogue[_dialogueIndex][0];
                    _dialogue[_dialogueIndex] = _dialogue[_dialogueIndex].Substring(1);
                    _timeSinceLastBlab = 0;
                }
                break;


            case DialogueMode.rush: //to change later once timing is implemented
                _panelAudioSource.pitch = 1.25f;
                if (!_panelAudioSource.isPlaying)
                {
                    _panelAudioSource.clip = SpeakerInfo.BlabNoises[0];
                    _panelAudioSource.Play();
                }

                // if not a stopping point, append next letter
                if (_timeSinceLastBlab >= RushTimeStep)
                {

                    _textMesh.text += _dialogue[_dialogueIndex][0];
                    _dialogue[_dialogueIndex] = _dialogue[_dialogueIndex].Substring(1);
                    _timeSinceLastBlab = 0;
                }
                break;


            case DialogueMode.race:
                _textMesh.text += _dialogue[_dialogueIndex];
                _dialogue[_dialogueIndex] = "";
                break;


            case DialogueMode.rest:
                if (Input.GetButtonDown("Jump"))
                {
                    _textMesh.text = "";
                    Panel.Mode = DialogueMode.ramble;

                    if (finished == CutsceneExitStatus.finished)
                    {
                        
                        return CutsceneExitStatus.finished;
                    }
                }
                break;
        }


        return CutsceneExitStatus.running;
    }

    
}
