using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

/// <summary>
/// UI class that handles the animation view
/// </summary>

public class AnimationTimelineView : MonoBehaviour
{
    [SerializeField]private TimelineBarView _timelineView;
    [SerializeField]private RectTransform _buttonRT;
    [SerializeField]private RectTransform _content;
    [SerializeField]private Button _addButton;
    [SerializeField]private AnimationSelectionView _animationSelectionPrefab;
    private AnimationSequenceRenderData _data;
    private List<AnimationSelectionView> _animationViews = new();
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
            view.transform.SetParent(_content, false);
            view.transform.SetAsLastSibling();
            view.SetData(_data.Timeline[i]);
            _animationViews.Add(view);
        }
        _buttonRT.SetAsLastSibling();
        _addButton.onClick.AddListener(_data.EventHandler.RespondToAddAnimation);
    }

    void OnDestroy()
    {
        _addButton.onClick.RemoveAllListeners();
    }
}


