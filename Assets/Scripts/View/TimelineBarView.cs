using UnityEngine;

public class TimelineBarView : MonoBehaviour
{
    [SerializeField]private ScaleBarView _scaleBarPrefab;
    private RectTransform _thisRT;
    private bool _initialized = false;

    void Awake()
    {
        _thisRT = (RectTransform)transform;
    }

    public void Initialize(int segmentsAmount)
    {
        if (_initialized)
            return;
          
        for (var i = 0; i<segmentsAmount; i++)
        {
            var go = Instantiate(_scaleBarPrefab);
            go.SetData(new ScaleBarRenderData{Number = i});
            go.transform.SetParent(transform);
        }
        _initialized = true;
    }
}
