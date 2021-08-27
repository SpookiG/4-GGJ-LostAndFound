using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;


/*
 * Expected Usage:
 * 
 * TimeLerp<float> tlerp = new TimeLerp<float>();
 * tlerp.Prep(from, to, 1, true, new SmoothStep());
 * tlerp.Set(0);
 * ...
 * (position, progress) = tlerp.Go();
 */

public class TimeLerp<T>
{
    private int _lerpTypeInt;
    private bool _running;
    private float _startTime;
    private object _from;
    private object _to;
    private float _speed;
    private float _progress;
    private bool _clamped;
    private Stepper _stepper;
    

    // make sure the type is float, Vector2, Vector3 or Vector4 and assign _lerpTypeInt as appropriate
    public TimeLerp()
    {
        Type type = typeof(T);

        if (type == typeof(float))
        {
            _lerpTypeInt = 1;
            return;
        }
        if (type == typeof(Vector2))
        {
            _lerpTypeInt = 2;
            return;
        }
        if (type == typeof(Vector3))
        {
            _lerpTypeInt = 3;
            return;
        }
        if (type == typeof(Vector4))
        {
            _lerpTypeInt = 4;
            return;
        }

        Exception e = new Exception("Type of lerp is not a float or unity Vector");
        throw (e);
    }

    public void Prep(T from, T to, float speed, bool clamped=true, Stepper stepper=null)
    {
        _from = from;
        _to = to;
        _speed = speed;
        _clamped = clamped;
        _stepper = stepper ?? new LinearStep();
        _progress = 0f;
        _running = false;
    }

    public void Set(float progress)
    {
        _progress = progress;
        _running = false;
    }

    public (T position, float progress) Go()
    {
        // first lerp the progress, this should stay linear
        _progress = Mathf.Clamp(_progress + (Time.deltaTime * _speed), 0, 1);
        float stepped = _stepper.Step(_progress);

        // then step this lerp based on an appropriate stepType
        object lerped;
        switch (_lerpTypeInt)
        {
            case 1:
                if (_clamped) lerped = Mathf.Lerp((float)_from, (float)_to, stepped);
                else lerped = Mathf.LerpUnclamped((float)_from,(float) _to, stepped);
                break;
            case 2:

                if (_clamped) lerped = Vector2.Lerp((Vector2)_from, (Vector2)_to, stepped);
                else lerped = Vector2.LerpUnclamped((Vector2)_from, (Vector2)_to, stepped);
                break;
            case 3:
                if (_clamped) lerped = Vector3.Lerp((Vector3)_from, (Vector3)_to, stepped);
                else lerped = Vector3.LerpUnclamped((Vector3)_from, (Vector3)_to, stepped);
                break;
            case 4:
                if (_clamped) lerped = Vector4.Lerp((Vector4)_from, (Vector4)_to, stepped);
                else lerped = Vector4.LerpUnclamped((Vector4)_from, (Vector4)_to, stepped);
                break;
            default:
                Exception e = new Exception("Type of lerp is not a float or unity Vector");
                throw (e);
        }
        return ((T)lerped, _progress);
    }
}



public class MultiTimeLerp<T>
{
    private List<object> _path;
    private int _noOfLerps;

    private int _lerpTypeInt;
    private bool _startOfInternalLerp;
    private float _startTime;
    private float _speed;
    private float _overallProgress;
    private Stepper _stepper;


    // make sure the type is float, Vector2, Vector3 or Vector4 and assign _lerpTypeInt as appropriate
    public MultiTimeLerp()
    {
        Type type = typeof(T);

        if (type == typeof(float))
        {
            _lerpTypeInt = 1;
            return;
        }
        if (type == typeof(Vector2))
        {
            _lerpTypeInt = 2;
            return;
        }
        if (type == typeof(Vector3))
        {
            _lerpTypeInt = 3;
            return;
        }
        if (type == typeof(Vector4))
        {
            _lerpTypeInt = 4;
            return;
        }

        Exception e = new Exception("Type of lerp is not a float or unity Vector");
        throw (e);
    }

    public void Prep(T[] path, float speed, Stepper stepper = null)
    {
        _path = new List<object>();
        foreach (T point in path)
        {
            _path.Add(point);
        }
        _noOfLerps = _path.Count - 1;

        _speed = speed;
        _stepper = stepper ?? new LinearStep();
        _overallProgress = 0f;
        _startOfInternalLerp = false;
    }

    public void Set(float progress)
    {
        _overallProgress = progress;
        _startOfInternalLerp = false;
    }

    public (T position, float progress) Go()
    {
        // first lerp the progress, this should stay linear
        _overallProgress = Mathf.Clamp(_overallProgress + (Time.deltaTime * _speed), 0, 1);
        
        float overallStepped = _stepper.Step(_overallProgress);

        float expandedStepped = overallStepped * _noOfLerps;
        int index = (int) Mathf.Floor(expandedStepped);
        float internalStepped = expandedStepped % 1;

        if (index >= _noOfLerps)
        {
            return ((T)_path[_path.Count - 1], 1);
        }


        // then step this lerp based on an appropriate stepType
        object lerped;
        switch (_lerpTypeInt)
        {
            case 1:
                lerped = Mathf.Lerp((float)_path[index], (float)_path[index + 1], internalStepped);
                break;
            case 2:
                lerped = Vector2.Lerp((Vector2)_path[index], (Vector2)_path[index + 1], internalStepped);
                break;
            case 3:
                lerped = Vector3.Lerp((Vector3)_path[index], (Vector3)_path[index + 1], internalStepped);
                break;
            case 4:
                lerped = Vector4.Lerp((Vector4)_path[index], (Vector4)_path[index + 1], internalStepped);
                break;
            default:
                Exception e = new Exception("Type of lerp is not a float or unity Vector");
                throw (e);
        }
        return ((T)lerped, _overallProgress);
    }
}








// a Stepper generally follows two important rules. progress = 0 => Step(progress) = 0 && progress = 1 => Step(progress) = 1
public interface Stepper
{
    float Step(float progress);
}


public class HardStep : Stepper
{
    public float Step(float progress)
    {
        return progress < 0.5f ? 0 : 1;
    }
}

public class LinearStep : Stepper
{
    public float Step(float progress)
    {
        return progress;
    }
}

public class SmoothStep : Stepper
{
    public float Step(float progress)
    {
        return Mathf.SmoothStep(0, 1, progress);
    }
}

public class PowStep : Stepper
{
    public float Step(float progress)
    {
        return Mathf.Pow(progress, 2);
    }
}


// like PowDown but slows properly when closer to (1, 1)
public class SlowToStop : Stepper
{
    public float Step(float progress)
    {
        return 2 / (1 + Mathf.Pow(100, -progress)) - 1;
    }
}

public class SinStep : Stepper
{
    public float Step(float progress)
    {
        return Mathf.Max(0, Mathf.Sin(Mathf.PI * progress));
    }
}

public class WibbleStep : Stepper
{
    public float Step(float progress)
    {
        return (Mathf.Sin((3 * progress * Mathf.PI) - (Mathf.PI / 2)) / 2) + 0.5f;
    }
}