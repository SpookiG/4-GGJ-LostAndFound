using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speakers", menuName = "Data Balls/Speakers")]
public class Speakers : ScriptableObject
{
    [HideInInspector]
    public Dictionary<string, SpeakerInfo> SpeakersLookup = new Dictionary<string, SpeakerInfo>();
    [SerializeField]
    private SpeakerInfo[] _speakers = new SpeakerInfo[] { };

    public void Init()
    {
        // convert array to a dict lookup
        foreach (SpeakerInfo info in _speakers)
        {
            SpeakersLookup[info.name] = info;
        }
    }
}
