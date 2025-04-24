using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionView : MonoBehaviour
{
    [SerializeField]private CharacterAvatarView _characterAvatarPrefab;
    [SerializeField]private AnimationTimelineView _animationView;
    [SerializeField]private RectTransform _content;
    private CharacterSelectionRenderData _renderData;
    private List<CharacterAvatarView> _views = new();

    public void SetData(CharacterSelectionRenderData data)
    {
        _renderData = data;
        foreach (var charName in _renderData.CharacterNames)
        {
            var charRenderData = new CharacterAvatarRenderData{
                Name = charName,
                EventHandler = data.EventHandler,
            };
            var charView = Instantiate(_characterAvatarPrefab).GetComponent<CharacterAvatarView>();
            charView.transform.SetParent(_content, false);
            charView.SetData(charRenderData);
            _views.Add(charView);
        }
    }

    public void UpdateDim(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            foreach (var charView in _views)
            {
                charView.EnableDim(false);
            }
            return;
        }
        
        foreach (var charView in _views)
        {
            charView.EnableDim(charView.Data.Name != name);
        }
    }
}

public struct CharacterSelectionRenderData
{
    public List<string> CharacterNames;
    public ICharacterAvatarEventHandler EventHandler;
}
