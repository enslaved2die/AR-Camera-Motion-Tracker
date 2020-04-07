using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class AnimJsonLoader : MonoBehaviour
{
    public TextAsset jsonInput;
    public TMPro.TMP_InputField inputField;
    public Button loadBtn, ClearBtn;

    [Space]
    private string animationName;
    public string assetPath = "Assets/_Animation/";
    [Space]
    public Animator targetAnimator;
    public AnimatorOverrideController animatorOverride;

    RootObject rootObject;
    [Space]
    public float detectedFrametime;
    public AnimationCurve xPos, yPos, zPos, xRot, yRot, zRot, wRot;
    [Space]
    public Vector3 posOffset;

    AnimationClip clip;
    Animator animator;

    private void Awake()
    {
        targetAnimator.enabled = false;
        inputField.text = "Drag and Drop a .animJson into the window!";
        loadBtn.interactable = false;
    }

    public void ClearFile()
    {
        if (targetAnimator.gameObject.GetComponent<Animation>())
            DestroyImmediate(targetAnimator.gameObject.GetComponent<Animation>());
        jsonInput = null;
        inputField.text = "Drag and Drop a .animJson into the window!";
        loadBtn.interactable = true;
    }

    [ContextMenu("Generate Animation Clip from String")]
    public void GenerateFromString()
    {
        GenerateAnimationClip(jsonInput.text);
    }

    public void GenerateFromFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        Debug.LogError(reader.ReadToEnd());
        GenerateAnimationClip(reader.ReadToEnd());
    }

    public void GenerateAnimationClip(string input)
    {
        animationName = jsonInput.name;


        rootObject = JsonUtility.FromJson<RootObject>(input);
        detectedFrametime = rootObject.frametime;

        clip = new AnimationClip();

        xPos = new AnimationCurve();
        yPos = new AnimationCurve();
        zPos = new AnimationCurve();

        xRot = new AnimationCurve();
        yRot = new AnimationCurve();
        zRot = new AnimationCurve();
        wRot = new AnimationCurve();


        for (int i = 0; i < rootObject.frameData.Count; i++)
        {
            xPos.AddKey(detectedFrametime * i, rootObject.frameData[i].position.x + posOffset.x);
            yPos.AddKey(detectedFrametime * i, rootObject.frameData[i].position.y + posOffset.y);
            zPos.AddKey(detectedFrametime * i, rootObject.frameData[i].position.z + posOffset.z);

            xRot.AddKey(detectedFrametime * i, rootObject.frameData[i].rotation.x);
            yRot.AddKey(detectedFrametime * i, rootObject.frameData[i].rotation.y);
            zRot.AddKey(detectedFrametime * i, rootObject.frameData[i].rotation.z);
            wRot.AddKey(detectedFrametime * i, rootObject.frameData[i].rotation.w);
        }

        targetAnimator.enabled = false;
        Animation anim = targetAnimator.gameObject.AddComponent(typeof(Animation)) as Animation;
        anim.playAutomatically = false;
        clip.legacy = true;
        clip.wrapMode = WrapMode.Loop;
        inputField.text = "Press Clear before you Load another File!";
        loadBtn.interactable = false;

        clip.SetCurve("", typeof(Transform), "localPosition.x", xPos);
        clip.SetCurve("", typeof(Transform), "localPosition.y", yPos);
        clip.SetCurve("", typeof(Transform), "localPosition.z", zPos);

        clip.SetCurve("", typeof(Transform), "localRotation.x", xRot);
        clip.SetCurve("", typeof(Transform), "localRotation.y", yRot);
        clip.SetCurve("", typeof(Transform), "localRotation.z", zRot);
        clip.SetCurve("", typeof(Transform), "localRotation.w", wRot);

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(clip, assetPath + animationName + ".anim");
        AssetDatabase.SaveAssets();
#endif

        animatorOverride["Base"] = clip;

        targetAnimator.runtimeAnimatorController = animatorOverride;

        anim.clip = clip;
        anim.AddClip(clip, clip.name);
        anim.Play();

        //targetAnimator.enabled = true;
    }

}
