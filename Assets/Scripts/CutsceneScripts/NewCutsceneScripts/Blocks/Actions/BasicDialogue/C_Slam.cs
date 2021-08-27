using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Basic Dialogue/Slam")]
public class C_Slam : C_BasicDialogue
{
    public char Delimiter = ' ';
    public float RambleTimeStep = 0.2f;
    public float RushTimeStep = 0.1f;

    private float _timeSinceLastSlam;


    public override void Init()
    {
        base.Init();
        _dialogueIndex = 0;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _timeSinceLastSlam = 0;
    }

    protected override CutsceneExitStatus _Run()
    {
        CutsceneExitStatus finished = base._Run();
        _timeSinceLastSlam += Time.deltaTime;


        switch (Panel.Mode)
        {
            case DialogueMode.ramble:
                _panelAudioSource.pitch = 1f;
                /*if (!_cameraAudioSource.isPlaying)
                {
                    _cameraAudioSource.clip = _speakerInfo.SlamNoises[0];
                    _cameraAudioSource.Play();
                }*/

                // if not a stopping point, append next letter
                if (_timeSinceLastSlam >= RambleTimeStep)
                {
                    _panelAudioSource.clip = SpeakerInfo.SlamNoises[0];
                    _panelAudioSource.Play();

                    int slamTo = _dialogue[_dialogueIndex].IndexOf(Delimiter);
                    if (slamTo >= 0)
                    {
                        _textMesh.text += _dialogue[_dialogueIndex].Substring(0, slamTo + 1);
                        _dialogue[_dialogueIndex] = _dialogue[_dialogueIndex].Substring(slamTo + 1);
                    }
                    else
                    {
                        _textMesh.text += _dialogue[_dialogueIndex];
                        _dialogue[_dialogueIndex] = "";
                    }
                    _timeSinceLastSlam = 0;
                }
                break;


            case DialogueMode.rush: //to change later once timing is implemented
                _panelAudioSource.pitch = 1.25f;
                /*if (!_cameraAudioSource.isPlaying)
                {
                    _cameraAudioSource.clip = _speakerInfo.SlamNoises[0];
                    _cameraAudioSource.Play();
                }*/

                // if not a stopping point, append next letter
                if (_timeSinceLastSlam >= RushTimeStep)
                {
                    _panelAudioSource.clip = SpeakerInfo.SlamNoises[0];
                    _panelAudioSource.Play();

                    int slamTo = _dialogue[_dialogueIndex].IndexOf(Delimiter);
                    if (slamTo >= 0)
                    {
                        _textMesh.text += _dialogue[_dialogueIndex].Substring(0, slamTo + 1);
                        _dialogue[_dialogueIndex] = _dialogue[_dialogueIndex].Substring(slamTo + 1);
                    }
                    else
                    {
                        _textMesh.text += _dialogue[_dialogueIndex];
                        _dialogue[_dialogueIndex] = "";
                    }
                    _timeSinceLastSlam = 0;
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
                        _panelAudioSource.Stop();
                        _dialogueIndex = 0;
                        return CutsceneExitStatus.finished;
                    }
                }
                break;
        }


        return CutsceneExitStatus.running;
    }
}
