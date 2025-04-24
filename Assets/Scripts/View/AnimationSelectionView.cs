using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSelectionView : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropDown;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Button _removeButton;
    [SerializeField] private RectTransform _animLengthParentRT;
    [SerializeField] private RectTransform _animLengthRT;
    private AnimationSelectionRenderData _data;
    private float _animParentWidth;

    public void SetData(AnimationSelectionRenderData data)
    {
        _data = data;
        _dropDown.ClearOptions();
        _dropDown.onValueChanged.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
        var optionList = new List<TMP_Dropdown.OptionData>();
        foreach (var name in _data.AnimationNames)
        {
            var optionData = new TMP_Dropdown.OptionData();
            optionData.text = name;
            optionList.Add(optionData);
        }
        _dropDown.AddOptions(optionList);
        _dropDown.SetValueWithoutNotify(_data.AnimationNames.IndexOf(_data.Name));
        _inputField.text = _data.Time.ToString();
        _dropDown.onValueChanged.AddListener(HandleDropdownValueChanged);
        _inputField.onValueChanged.AddListener(HandleOnInputDurationChanged);
        _removeButton.onClick.AddListener(HandleOnRemoveButtonPressed);
        StartCoroutine(CacheWidthAndSetAnimLengthPanel());
    }

    private IEnumerator CacheWidthAndSetAnimLengthPanel()
    {
        yield return null;
        _animParentWidth = _animLengthParentRT.rect.width;
        SetAnimLengthPanel();
    }

    private void SetAnimLengthPanel()
    {
        var pos = _animParentWidth / _data.MaxDuration * _data.Time;
        var animLengthWidth = _animParentWidth / _data.MaxDuration * _data.Length;
        _animLengthRT.anchoredPosition = new Vector2(pos, 0);
        _animLengthRT.sizeDelta = new Vector2 (animLengthWidth, _animLengthRT.sizeDelta.y);
    }

    void OnDestroy()
    {
        _dropDown.onValueChanged.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
        _removeButton.onClick.RemoveAllListeners();
    }

    private void HandleDropdownValueChanged(int val)
    {
        _data.EventHandler.RespondToAnimationChanged(_data.Index, _data.AnimationNames[val], ref _data);
        SetAnimLengthPanel();
    }

    private void HandleOnInputDurationChanged(string val)
    {
        if (float.TryParse(val, out var floatVal))
        {
            _data.Time = floatVal;
            SetAnimLengthPanel();
            _data.EventHandler.RespondToAnimationDurationChanged(_data.Index, floatVal);
        }
    }

    private void HandleOnRemoveButtonPressed()
    {
        _data.EventHandler.RespondToAnimationRemoved(_data.Index);
        Destroy(gameObject);
    }
}

public class AnimationSelectionRenderData
{
    public List<string> AnimationNames;
    public string Name;
    public float Time;
    public float Length;
    public int Index;
    public int MaxDuration;
    public IAnimationSelectionEventHandler EventHandler;
}

public interface IAnimationSelectionEventHandler
{
    void RespondToAnimationChanged(int index, string name, ref AnimationSelectionRenderData data);
    void RespondToAnimationDurationChanged (int index, float newDuration);
    void RespondToAnimationRemoved(int index);
}
