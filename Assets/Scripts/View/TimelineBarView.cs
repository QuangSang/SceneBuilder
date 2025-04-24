using TMPro;
using UnityEngine;

/// <summary>
/// UI class that populate the timeline bar
/// </summary>
public class TimelineBarView : MonoBehaviour
{
    [SerializeField]private ScaleBarView _scaleBarPrefab;
    private RectTransform _thisRT;
    private bool _initialized = false;
    private int _currentSegment;

    void Awake()
    {
        _thisRT = (RectTransform)transform;
    }

    public void Initialize(int segmentsAmount)
    {
        if(_currentSegment == segmentsAmount)
        {
            return;
        }

        foreach (RectTransform child in transform)
        {
            Destroy(child.gameObject);
        }
          
        for (var i = 0; i<segmentsAmount; i++)
        {
            var go = Instantiate(_scaleBarPrefab);
            go.SetData(new ScaleBarRenderData{Number = i});
            go.transform.SetParent(transform, false);
        }
        _initialized = true;
    }
}
