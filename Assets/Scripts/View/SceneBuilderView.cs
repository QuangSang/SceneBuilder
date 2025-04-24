using UnityEngine;
using UnityEngine.UI;

public class SceneBuilderView : MonoBehaviour
{
    [SerializeField] private CharacterSelectionView _selectionView;
    [SerializeField] private AnimationTimelineView _animationView;
    [SerializeField] private Button _placeButton;
    [SerializeField] private RectTransform _moreButtonRT;
    [SerializeField] private CanvasGroup _moreButtonCG;
    [SerializeField] private Button _addMoreButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _closeButton;

    public void SetData(SceneBuilderRenderData renderData)
    {
        _placeButton.onClick.AddListener(renderData.EventHandler.RespondToPlaceButtonPressed);
        _addMoreButton.onClick.AddListener(Show);
        _playButton.onClick.AddListener(renderData.EventHandler.RespondToPlayButtonPressed);
        _resetButton.onClick.AddListener(renderData.EventHandler.RespondToResetButtonPressed);
        _saveButton.onClick.AddListener(renderData.EventHandler.RespondToSaveButtonPressed);
        _loadButton.onClick.AddListener(renderData.EventHandler.RespondToLoadButtonPressed);
        _clearButton.onClick.AddListener(renderData.EventHandler.RespondToClearButtonPressed);
        _closeButton.onClick.AddListener(Hide);
        EnableDisablePlaceButton(false);
    }

    void OnDestroy()
    {
        _placeButton?.onClick.RemoveAllListeners();
        _addMoreButton?.onClick.RemoveAllListeners();
        _playButton?.onClick.RemoveAllListeners();
        _resetButton?.onClick.RemoveAllListeners();
        _saveButton?.onClick.RemoveAllListeners();
        _loadButton?.onClick.RemoveAllListeners();
        _clearButton?.onClick.RemoveAllListeners();
        _closeButton?.onClick.RemoveAllListeners();
    }
    public void ShowSelectionView(CharacterSelectionRenderData renderData)
    {
        _selectionView.gameObject.SetActive(true);
        _selectionView.SetData(renderData);
    }

    public void ShowAnimationView(AnimationSequenceRenderData renderData)
    {
        _animationView.gameObject.SetActive(true);
        _animationView.SetData(renderData);
        EnableDisablePlaceButton(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _moreButtonRT.gameObject.SetActive(true);
        EnableDisableScenePlayerButton(true);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _selectionView.gameObject.SetActive(true);
        _selectionView.UpdateDim("");
        _animationView.gameObject.SetActive(false);
        _placeButton.gameObject.SetActive(true);
        EnableDisablePlaceButton(false);
        _moreButtonRT.gameObject.SetActive(false);
    }

    public void EnableDisablePlaceButton(bool enabled)
    {
        _placeButton.interactable = enabled;
    }

    public void EnableDisableScenePlayerButton(bool enabled)
    {
        _moreButtonCG.interactable = enabled;
    }

    public void HighlightSelectedCharacter(string name)
    {
        _selectionView.UpdateDim(name);
    }
}

public class SceneBuilderRenderData
{
    public int MaxAnimationDuration;
    public ISceneBuilderEventHandler EventHandler;
}

public interface ISceneBuilderEventHandler
{
    void RespondToPlaceButtonPressed();
    void RespondToPlayButtonPressed();
    void RespondToResetButtonPressed();
    void RespondToSaveButtonPressed();
    void RespondToLoadButtonPressed();
    void RespondToClearButtonPressed();
}
