using TMPro;
using UnityEngine;

public class TextInputHandler : MonoBehaviour
{
    public AnimJsonLoader loader;
    public TMP_InputField inputField;

    public void Submit()
    {
        loader.GenerateAnimationClip(inputField.text);
    }

    public void ClearText()
    {
        inputField.text = "";
    }
}
