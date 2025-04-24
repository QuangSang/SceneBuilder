using TMPro;
using UnityEngine;

public class ScaleBarView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    public void SetData(ScaleBarRenderData data)
    {
        _label.SetText(data.Number.ToString());
    }
    
}

public struct ScaleBarRenderData
{
    public int Number;
}
