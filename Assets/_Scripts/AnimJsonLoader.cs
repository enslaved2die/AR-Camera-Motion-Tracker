using UnityEditor;
using UnityEngine;

public class AnimJsonLoader : MonoBehaviour
{
    public RootObject rootObject;

    public string jsonInput;

    public float frametime;

    public AnimationCurve xPos, yPos, zPos, xRot, yRot, zRot, wRot;
    public Vector3 posOffset;

    public AnimationClip clip;
    public Animator animator;

    public Transform retarget;

    void Start()
    {
        rootObject = JsonUtility.FromJson<RootObject>(jsonInput);
        frametime = rootObject.frametime;

        clip = new AnimationClip();

        xPos = new AnimationCurve();
        yPos = new AnimationCurve();
        zPos = new AnimationCurve();


        for (int i = 0; i < rootObject.frameData.Count; i++)
        {
            xPos.AddKey(frametime * i, rootObject.frameData[i].position.x + posOffset.x);
            yPos.AddKey(frametime * i, rootObject.frameData[i].position.y + posOffset.y);
            zPos.AddKey(frametime * i, rootObject.frameData[i].position.z + posOffset.z);

            xRot.AddKey(frametime * i, rootObject.frameData[i].rotation.x);
            yRot.AddKey(frametime * i, rootObject.frameData[i].rotation.y);
            zRot.AddKey(frametime * i, rootObject.frameData[i].rotation.z);
            wRot.AddKey(frametime * i, rootObject.frameData[i].rotation.w);
        }

        clip.SetCurve("", typeof(Transform), "localPosition.x", xPos);
        clip.SetCurve("", typeof(Transform), "localPosition.y", yPos);
        clip.SetCurve("", typeof(Transform), "localPosition.z", zPos);

        clip.SetCurve("", typeof(Transform), "localRotation.x", xRot);
        clip.SetCurve("", typeof(Transform), "localRotation.y", yRot);
        clip.SetCurve("", typeof(Transform), "localRotation.z", zRot);
        clip.SetCurve("", typeof(Transform), "localRotation.w", wRot);


#if UNITY_EDITOR
        AssetDatabase.CreateAsset(clip, "Assets/_Animation/clip.anim");
        AssetDatabase.SaveAssets();
#endif


    }

}
