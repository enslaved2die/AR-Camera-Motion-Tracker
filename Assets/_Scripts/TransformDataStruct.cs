using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrameData
{
    public int frame;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[System.Serializable]
public class RootObject
{
    public List<FrameData> frameData;
    public float frametime;
}
