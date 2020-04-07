using UnityEngine;
using UnityEngine.UI;

public class FPSSelection : MonoBehaviour
{
    public TransformRecorder transformRecorder;
    public GameObject[] btns;
    public GameObject holder;
    public RectTransform settingsBtn;

    private void Awake()
    {
        holder.SetActive(false);
        UpdateFPSDisplay();
    }

    public void ToggleSettings()
    {
        if (holder.activeInHierarchy)
        {
            holder.SetActive(false);
            settingsBtn.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            holder.SetActive(true);
            settingsBtn.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    public void switchFPS(int id)
    {
        transformRecorder.frameRate = (TransformRecorder.FrameRates)id;
        UpdateFPSDisplay();
    }

    void UpdateFPSDisplay()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].GetComponentInChildren<Image>().enabled = false;
        }

        switch (transformRecorder.frameRate)
        {
            case TransformRecorder.FrameRates._10:
                btns[0].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._23_976:
                btns[1].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._24:
                btns[2].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._25:
                btns[3].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._29_97:
                btns[4].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._30:
                btns[5].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._40:
                btns[6].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._48:
                btns[7].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._50:
                btns[8].GetComponentInChildren<Image>().enabled = true;
                break;
            case TransformRecorder.FrameRates._60:
                btns[9].GetComponentInChildren<Image>().enabled = true;
                break;
            default:
                break;
        }
    }
}
