using TMPro;
using UnityEngine;

public struct ScaleBarRenderData
{
    public int Number;
}

/// <summary>
/// UI class that populate the individual bar in the timeline
/// </summary>
public class ScaleBarView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    public void SetData(ScaleBarRenderData data)
    {
        _label.SetText(data.Number.ToString());
    }
    
}


