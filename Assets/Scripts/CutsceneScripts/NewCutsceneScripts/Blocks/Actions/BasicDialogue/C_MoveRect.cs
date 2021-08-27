using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[AddComponentMenu("Cutscene/Blocks/Actions/Basic Dialogue/Move Rect")]
public class C_MoveRect : C_BasicDialogue
{
    // TODO think about it
    public float XSpeed = 1;
    public float YSpeed = 1;

    public C_MoveRectVec[] Ranges;
    

    private RectTransform _rect;
    private MultiTimeLerp<Vector2> _XLerp;
    private MultiTimeLerp<Vector2> _YLerp;
    //private bool _start;


    public override void Init()
    {
        base.Init();
        _start = true;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _rect = Panel.Rect;// _dialoguePanel.GetComponent<RectTransform>();
        _XLerp = new MultiTimeLerp<Vector2>();
        _YLerp = new MultiTimeLerp<Vector2>();
        _start = true;
    }


    protected override CutsceneExitStatus _Run()
    {
        if (_start)
        {
            List<Vector2> travelX = new List<Vector2>();
            List<Vector2> travelY = new List<Vector2>();

            foreach (C_MoveRectVec range in Ranges)
            {
                if (range.XType == MoveRectVecType.pos)
                {
                    travelX.Add(new Vector2(_rect.anchorMin.x, _rect.anchorMax.x));
                }
                else
                {
                    travelX.Add(range.XVec);
                }

                if (range.YType == MoveRectVecType.pos)
                {
                    travelY.Add(new Vector2(_rect.anchorMin.y, _rect.anchorMax.y));
                }
                else
                {
                    travelY.Add(range.YVec);
                }
            }

            _XLerp.Prep(travelX.ToArray(), XSpeed, new LinearStep());
            _YLerp.Prep(travelY.ToArray(), YSpeed, new LinearStep());
            _start = false;
        }

        var x = _XLerp.Go();
        var y = _YLerp.Go();
        _rect.anchorMin = new Vector2(x.position.x, y.position.x);
        _rect.anchorMax = new Vector2(x.position.y, y.position.y);

        if (x.progress == 1 && y.progress == 1)
        {
            return CutsceneExitStatus.finished;
        }

        return CutsceneExitStatus.running;
    }
}

public enum MoveRectVecType
{
    custom,
    pos,
    preset //TODO: for later
}

[Serializable]
public class C_MoveRectVec
{
    public MoveRectVecType XType;
    public MoveRectVecType YType;
    public Vector2 XVec;
    public Vector2 YVec;
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(C_MoveRectVec))]
public class C_MoveRectVecDrawerUIE : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var XLabelRect = new Rect(position.x, position.y, 50, 20);
        var XTypeRect = new Rect(position.x + 55, position.y, 80, 20);
        var XVecRect = new Rect(position.x + 140, position.y, 100, 20);
        var YLabelRect = new Rect(position.x, position.y + 20, 50, 20);
        var YTypeRect = new Rect(position.x + 55, position.y + 20, 80, 20);
        var YVecRect = new Rect(position.x + 140, position.y + 20, 100, 20);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.LabelField(XLabelRect, "X Range", EditorStyles.boldLabel);
        EditorGUI.PropertyField(XTypeRect, property.FindPropertyRelative("XType"), GUIContent.none);
        if (property.FindPropertyRelative("XType").enumValueIndex == 0)
        {
            EditorGUI.PropertyField(XVecRect, property.FindPropertyRelative("XVec"), GUIContent.none);
        }

        EditorGUI.LabelField(YLabelRect, "Y Range", EditorStyles.boldLabel);
        EditorGUI.PropertyField(YTypeRect, property.FindPropertyRelative("YType"), GUIContent.none);
        if (property.FindPropertyRelative("YType").enumValueIndex == 0)
        {
            EditorGUI.PropertyField(YVecRect, property.FindPropertyRelative("YVec"), GUIContent.none);
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 50;
    }
}

#endif