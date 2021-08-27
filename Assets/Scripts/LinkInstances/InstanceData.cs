using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// current instance ID has to be held in a ScriptableObject otherwise the unity editor keeps resetting it every time the game is stopped in the inspector
[CreateAssetMenu(fileName = "InstanceData", menuName = "Instance Data")]
public class InstanceData : ScriptableObject
{
    public int ID;
}
