using B83.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileDragAndDrop : MonoBehaviour
{
    List<string> log = new List<string>();
    void OnEnable()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        string path = aFiles.Aggregate((a, b) => a + "\n\t" + b);

        StreamReader reader = new StreamReader(path);
        TextAsset text = new TextAsset(reader.ReadToEnd());

        GetComponent<AnimJsonLoader>().jsonInput = text;
        GetComponent<AnimJsonLoader>().inputField.text = "File Loaded, Press Load to Continue!";
        GetComponent<AnimJsonLoader>().loadBtn.interactable = true;
    }
}
