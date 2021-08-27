using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// Use this to reset game data for testing

public class GameRefresher : MonoBehaviour
{
    public GameLoader Loader;
    // because the unity editor may not call Application.Quit() (eg if stopping the game with the stop button) we may need to refresh the saved game data

    // use to test what happens when the game exits.
    public void Refresh()
    {
        Loader.CloseUp();
    }

    // use clear data to start from scratch
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
}


// Unity won't compile this into an exe
#if UNITY_EDITOR

[CustomEditor(typeof(GameRefresher))]
public class GameRefresherDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Refresh"))
        {
            var refresher = (GameRefresher)target;

            refresher.Refresh();
        }

        if (GUILayout.Button("Clear Data"))
        {
            var refresher = (GameRefresher)target;

            refresher.ClearData();
        }
    }
}

#endif