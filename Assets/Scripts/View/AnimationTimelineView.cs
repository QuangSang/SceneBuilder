using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTimelineView : MonoBehaviour
{
    [SerializeField]private TimelineBarView _timelineView;
    [SerializeField]private RectTransform _buttonRT;
    [SerializeField]private RectTransform _content;
    [SerializeField]private Button _addButton;
    [SerializeField]private AnimationSelectionView _animationSelectionPrefab;
    private AnimationSequenceRenderData _data;
    public void SetData(AnimationSequenceRenderData data)
    {
        _data = data;
        _addButton.onClick.RemoveAllListeners();
        _timelineView.Initialize(_data.MaxAnimationDuration);
        foreach (RectTransform child in _content)
        {
            if (child != _buttonRT)
                Destroy(child.gameObject);
        }

        for(int i = 0; i< _data.Timeline.Count ; i++)
        {
            var view = Instantiate(_animationSelectionPrefab);
            view.transform.SetParent(_content);
            view.transform.SetAsLastSibling();
            view.SetData(_data.Timeline[i]);
        }
        _buttonRT.SetAsLastSibling();
        _addButton.onClick.AddListener(_data.EventHandler.RespondToAddAnimation);
    }

    void OnDestroy()
    {
        _addButton.onClick.RemoveAllListeners();
    }
}

public class AnimationSequenceRenderData
{
    public int MaxAnimationDuration;
    public List<AnimationSelectionRenderData> Timeline = new();
    public IAnimationTimelineEventHandler EventHandler;
}

public interface IAnimationTimelineEventHandler
{
    void RespondToAddAnimation();
}
