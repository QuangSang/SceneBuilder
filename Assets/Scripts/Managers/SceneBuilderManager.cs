using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneBuilderManager : Singleton<SceneBuilderManager>,  
                                    ICharacterAvatarEventHandler, 
                                    IAnimationTimelineEventHandler, 
                                    IAnimationSelectionEventHandler,
                                    ISceneBuilderEventHandler
{
    [SerializeField] private CharacterDefinition _characterDefinition;
    [SerializeField] private SceneBuilderView _sceneBuilderView;
    [SerializeField] private int _maxAnimationDuration = 20;

    private CharacterData _currentCharacterData;
    private CharacterAnimationSequenceData _sequenceData;
    private ResourceLoader _resourceLoader;
    private ScenePlayerManager _scenePlayer;
    private SceneData _sceneData;

    protected override void Awake()
    {
        base.Awake();
        _resourceLoader = new ResourceLoader();
        _sceneData = new SceneData();
        _scenePlayer = ScenePlayerManager.Instance;
        _scenePlayer.Initialize(_resourceLoader);
    }

    void Start()
    {
        var sceneBuilderRenderData = new SceneBuilderRenderData{
            EventHandler = this,
        };
        _sceneBuilderView.SetData(sceneBuilderRenderData);
        var renderData = CreateCharacterSelectionRenderData();
        _sceneBuilderView.ShowSelectionView(renderData);
    }

    private CharacterSelectionRenderData CreateCharacterSelectionRenderData()
    {
        var renderData = new CharacterSelectionRenderData();
        renderData.CharacterNames = new();
        renderData.EventHandler = this;
        foreach (var d in _characterDefinition.Definitions)
        {
            renderData.CharacterNames.Add(d.Name);
        }
        return renderData;
    }

    private AnimationSequenceRenderData CreateAnimationSequenceRenderData()
    {
        var animationNames = _currentCharacterData.GetAnimationNames();
        var renderData = new AnimationSequenceRenderData();
        renderData.EventHandler = this;
        renderData.MaxAnimationDuration = _maxAnimationDuration;
        for (int i = 0; i< _sequenceData.Sequence.Count; i++)
        {
            var anim = _sequenceData.Sequence[i];
            var animRenderData = new AnimationSelectionRenderData{
                AnimationNames = new List<string>(animationNames),
                Name = anim.AnimationName,
                Time = anim.Time,
                Length = _currentCharacterData.GetAnimationLength(anim.AnimationName),
                MaxDuration = _maxAnimationDuration,
                Index = i,
                EventHandler = this,
            };

            renderData.Timeline.Add(animRenderData);
        }
        return renderData;
    }


    public void RespondToCharacterAvatarPressed(string name)
    {
        _currentCharacterData = _characterDefinition.Definitions.FirstOrDefault(d=>d.Name == name);
        _sequenceData = new CharacterAnimationSequenceData(name);
        RefreshAnimationView();
        _sceneBuilderView.HighlightSelectedCharacter(name);
    }

    public void RespondToAddAnimation()
    {
        var animNames = _currentCharacterData.GetAnimationNames();
        _sequenceData.AddAnimation (new AnimationData{
            AnimationName = animNames[0],
            Time = 0f,
            Loop = false
        });
        RefreshAnimationView();
    }
    private void RefreshAnimationView()
    {
        if (_currentCharacterData == null)
            return;
        
        var renderData = CreateAnimationSequenceRenderData();
        _sceneBuilderView.ShowAnimationView(renderData);
    }

    private async void RespondToPlacingCharacter(Vector3 position, Quaternion rotation)
    {
        var character =  await _resourceLoader.InstantiateAsyncGameObject<CharacterAnimationController>(_currentCharacterData.Asset, position, rotation, null);
        var anim = character.GetComponent<Animator>();
        anim.enabled = false;
        anim.runtimeAnimatorController = _currentCharacterData.Animator;
        _sequenceData.Position = character.transform.position;
        _sequenceData.Rotation = character.transform.rotation;
        character.SetData(_sequenceData);
        _scenePlayer.RegisterActor(character);
        _sceneData.AddActor(_sequenceData);
    }

    public void RespondToAnimationChanged(int index, string name, ref AnimationSelectionRenderData renderData)
    {
        _sequenceData.UpdateAnimationName(index, name);
        renderData.Name = name;
        renderData.Length = _currentCharacterData.GetAnimationLength(name);
    }

    public void RespondToAnimationDurationChanged(int index, float newDuration)
    {
        _sequenceData.UpdateAnimationDuration(index, newDuration);
    }

    public async void RespondToPlaceButtonPressed()
    {
        if (_currentCharacterData == null)
            return;
        
        _sceneBuilderView.Hide();
        var miniChar = await _resourceLoader.InstantiateAsyncGameObject<FollowMouseComponent>(_currentCharacterData.AssetMini, Vector3.zero, Quaternion.identity, null);
        miniChar.Initialize(_resourceLoader, RespondToPlacingCharacter, RespondToCancelPlacingCharacter);
        miniChar.enabled = true;
    }

    private void RespondToCancelPlacingCharacter()
    {

    }

    public void RespondToAddMoreButtonPressed()
    {
        _sceneBuilderView.Show();
    }

    public void RespondToPlayButtonPressed()
    {
        _scenePlayer.Play();
    }
    public void RespondToResetButtonPressed()
    {
        _scenePlayer.Reset();
    }
    public void RespondToAnimationRemoved(int index)
    {
        _sequenceData.RemoveAnimation(index);
    }
}
