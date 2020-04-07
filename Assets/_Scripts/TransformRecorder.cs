using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransformRecorder : MonoBehaviour
{
    public Transform recordingTarget;

    [Header("Look")]
    public Sprite startRecSprite;
    public Sprite stopRecSprite;
    public Image recBtn;
    public TextMeshProUGUI outputText, transformOutputText;

    [Space]
    public RootObject rootObject;

    public string SAVE_FOLDER;
    public string FILE_NAME;

    List<float> frameRates = new List<float>() { 24, 25, 30, 50, 60 };
    [System.Serializable]
    public enum FrameRates { _10, _23_976, _24, _25, _29_97, _30, _40, _48, _50, _60 };

    public FrameRates frameRate;
    float Rate;
    float currentFrameTime;
    int recordedFrames;

    public bool isRecording = false;
    public bool saveOnStop = true;

    [TextArea(2, 100)]
    public string json;

    public void RecordingToggle()
    {
        //StartRecording
        if (!isRecording)
        {
            StartCoroutine("WaitForNextFrame");
            recBtn.sprite = stopRecSprite;
        }
        //Stop Recording
        else
        {
            recBtn.sprite = startRecSprite;
            StopCoroutine("WaitForNextFrame");
            isRecording = false;
            if (saveOnStop)
                StartCoroutine(SaveToJson());
        }
    }

    IEnumerator SaveToJson()
    {
        json = JsonUtility.ToJson(rootObject);

        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }


        File.WriteAllText(SAVE_FOLDER + FILE_NAME, json);

        yield return new WaitUntil(() => File.Exists(SAVE_FOLDER + FILE_NAME));

        outputText.text = "File Saved:\n" + SAVE_FOLDER + FILE_NAME;

    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 9999;
        currentFrameTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        transformOutputText.text = "World:\n" + recordingTarget.position + "\n" + recordingTarget.eulerAngles + "\nRecorded" + rootObject.frameData[rootObject.frameData.Count - 1].position + "\n" + rootObject.frameData[rootObject.frameData.Count - 1].rotation.eulerAngles;

    }

    void SaveTranslationAndRotation()
    {
        FrameData temp = new FrameData();

        temp.frame = recordedFrames;
        temp.position = recordingTarget.localPosition;
        temp.rotation = recordingTarget.localRotation;
        temp.scale = recordingTarget.localScale;

        rootObject.frameData.Add(temp);
        rootObject.frametime = 1.0f / Rate;
    }

    IEnumerator WaitForNextFrame()
    {
        switch (frameRate)
        {
            case FrameRates._10:
                Rate = 10f;
                break;
            case FrameRates._23_976:
                Rate = 23.976f;
                break;
            case FrameRates._24:
                Rate = 24f;
                break;
            case FrameRates._25:
                Rate = 25f;
                break;
            case FrameRates._29_97:
                Rate = 29.97f;
                break;
            case FrameRates._30:
                Rate = 30f;
                break;
            case FrameRates._40:
                Rate = 40f;
                break;
            case FrameRates._48:
                Rate = 48f;
                break;
            case FrameRates._50:
                Rate = 50f;
                break;
            case FrameRates._60:
                Rate = 60f;
                break;
            default:
                break;
        }

        isRecording = true;
        recordedFrames = 0;

        SAVE_FOLDER = Application.persistentDataPath + "/Recordings/" + System.DateTime.Now.ToString("yyyy-MM-dd");
        FILE_NAME = "/" + System.DateTime.Now.ToString("HH-mm-ss") + "_" + Rate.ToString().Replace(",", "_") + "p.json";

        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / Rate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;

            recordedFrames++;

            SaveTranslationAndRotation();
        }
    }
}
