using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class C_BasicDialogue : C_CutsceneBlock
{
    protected static Dictionary<string, DialogueMode> Modes = new Dictionary<string, DialogueMode>();

    //public Canvas CutsceneCanvas;
    //public GameObject PanelPrefab;      // TODO: add programatically
    //public string DialogueBoxName;
    public SpeakerInfo SpeakerInfo;     // core functionality
    public C_DialoguePanel Panel;

    [TextArea(0, 8)]
    public string[] Dialogue; // TODO: change this maybe?


    protected int _dialogueIndex;
    protected string[] _dialogue;
    //protected GameObject _dialoguePanel;
    protected TextMeshProUGUI _textMesh;
    protected AudioSource _panelAudioSource;

    protected bool _start;


    public override void Init()
    {
        _start = true;
        _dialogue = (from line in Dialogue select line).ToArray();
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        _dialogueIndex = 0;
        _dialogue = (from line in Dialogue select line).ToArray();
        _start = true;

        _textMesh = Panel.TextMesh;
        _panelAudioSource = Panel.PanelAudioSource;


        /*_dialoguePanel = GetDialoguePanel(DialogueBoxName);
        _textMesh = _dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
        _panelAudioSource = _dialoguePanel.GetComponent<AudioSource>();*/

        //Panel.Mode = DialogueMode.ramble;
    }

    protected override CutsceneExitStatus _Run()
    {
        if (_dialogueIndex >= _dialogue.Length)
        {
            Panel.Mode = DialogueMode.rest;
            return CutsceneExitStatus.finished;
        }

        if (!_start)
        {
            // I had to add this as it's the only reliable way of changing the state from a single place pls don't criticise me lol
            switch (Panel.Mode)
            {
                case DialogueMode.ramble:
                    if (Input.GetButtonDown("Jump"))
                    {
                        Panel.Mode = DialogueMode.rush;
                    }
                    break;
                case DialogueMode.rush:
                    if (Input.GetButtonUp("Jump"))
                    {
                        Panel.Mode = DialogueMode.race;
                    }
                    break;
            }
        }

        _start = false;

        // Basically an empty string makes this wait / pass on to the next block ok this is workable
        // check if we're at a stopping point (at the end of the current string) and if there is more dialogue to wait for
        if (Panel.Mode != DialogueMode.rest && _dialogue[_dialogueIndex].Length <= 0)
        {
            Panel.Mode = DialogueMode.rest;
            _dialogueIndex++;
        }

        return CutsceneExitStatus.running;
    }

    


    /*public GameObject GetDialoguePanel(string name)
    {
        GameObject panel = CutsceneCanvas.transform.Find(name)?.gameObject;
        if (panel == null)
        {
            panel = GameObject.Instantiate(SpeakerInfo.DialoguePanelPrefab, CutsceneCanvas.transform);
            panel.name = name;
        }
        return panel;
    }*/


    // TODO: add some generic functions here for end of dialogue
    // possiblilities
        // wait for input then clear box
        // wait for input then keep adding to dialogue in box
        // wait for an amount of time before continuing
        // don't wait, go straight to the next box

    // in this case
        // enum for clearing box
        // enum for waiting for next bit of dialogue




    // As a different type of BasicDialogue: maybe we don't want to wait for a button press, maybe we just want to wait a bit before continuing dialogue (if doing the thing above w/ keeping current dialogue)
}


public enum DialogueMode
{
    ramble,
    rush,
    race,
    rest
}