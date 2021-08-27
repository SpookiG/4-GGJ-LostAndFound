using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerInfo", menuName = "Data Balls/Speaker Info")]
public class SpeakerInfo : ScriptableObject
{
    public GameObject DialoguePanelPrefab;
    public AudioClip[] BlabNoises;
    public AudioClip[] SlamNoises;
}
