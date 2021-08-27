using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[AddComponentMenu("Cutscene/Dialogue Panel")]
public class C_DialoguePanel : MonoBehaviour
{
    [HideInInspector] public DialogueMode Mode;
    [HideInInspector] public TextMeshProUGUI TextMesh
    {
        get
        {
            _textMesh = _textMesh != null ? _textMesh : GetComponentInChildren<TextMeshProUGUI>();
            return _textMesh;
        }
        private set
        {
            _textMesh = value;
        }
    }
    [HideInInspector] public AudioSource PanelAudioSource
    {
        get
        {
            _panelAudioSource = _panelAudioSource != null ? _panelAudioSource : GetComponent<AudioSource>();
            return _panelAudioSource;
        }
        private set
        {
            _panelAudioSource = value;
        }
    }
    [HideInInspector] public RectTransform Rect
    {
        get
        {
            _rect = _rect != null ? _rect : GetComponent<RectTransform>();
            return _rect;
        }
        private set
        {
            _rect = value;
        }
    }


    private TextMeshProUGUI _textMesh;
    private AudioSource _panelAudioSource;
    private RectTransform _rect;


    // Start is called before the first frame update
    void Start()
    {
        Mode = DialogueMode.ramble;
    }
}
