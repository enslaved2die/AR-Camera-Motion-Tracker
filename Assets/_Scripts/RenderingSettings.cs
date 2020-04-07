using UnityEngine;

public class RenderingSettings : MonoBehaviour
{
    public int vSyncCount = 1;
    private void Awake()
    {
        QualitySettings.vSyncCount = vSyncCount;
    }
}
