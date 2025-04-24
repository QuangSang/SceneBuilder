using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct CharacterAvatarRenderData
{
    public string Name;
    public ICharacterAvatarEventHandler EventHandler;
}

public interface ICharacterAvatarEventHandler
{
    void RespondToCharacterAvatarPressed(string name);
}

/// <summary>
/// UI class that updates the view of the character button
/// </summary>

public class CharacterAvatarView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _tintRT;
    public CharacterAvatarRenderData Data => _data;


    private CharacterAvatarRenderData _data;

    public void SetData(CharacterAvatarRenderData data)
    {
        _data = data;
        _label.SetText(_data.Name);
        _button.onClick.AddListener(SpawnCharacter);
    }

    public void Shutdown()
    {
        _button.onClick.RemoveListener(SpawnCharacter);
    }

    void OnDestroy()
    {
        Shutdown();
    }

    private void SpawnCharacter()
    {
        _data.EventHandler.RespondToCharacterAvatarPressed(_data.Name);
    }

    public void EnableDim(bool enabled)
    {
        _tintRT.gameObject.SetActive(enabled);
    }
    
}


