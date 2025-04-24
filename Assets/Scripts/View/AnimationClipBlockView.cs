using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationClipBlockRenderData
{
    public float ParentWidth;
}
public class AnimationClipBlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<float> OnPositionChanged;
    public Action<float> OnFinalPositionChanged;
    private RectTransform _rectTransform;
    private AnimationClipBlockRenderData _data;
    private Vector2 _dragOffset;
    private float _currentPos;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetData(AnimationClipBlockRenderData renderData)
    {
        _data = renderData;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
         RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            null,
            out var localMousePos
        );

        _dragOffset = localMousePos - _rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            null,
            out var localMousePos
        );

        var clampedX = Mathf.Clamp(localMousePos.x - _dragOffset.x, 0, _data.ParentWidth);

        _rectTransform.anchoredPosition = new Vector2(clampedX, 0);
        _currentPos = clampedX;
        OnPositionChanged?.Invoke(_rectTransform.anchoredPosition.x);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnFinalPositionChanged?.Invoke(_currentPos);
    }
}
