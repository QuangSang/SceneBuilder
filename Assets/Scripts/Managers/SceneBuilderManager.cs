using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Top level manager class for the scene builder. Responsible for:
///     - Initializing the UI
///     - Updating the scene data
///     - Spawning characters
/// </summary>
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
    private SceneLoader _sceneLoader;
    private SceneData _sceneData;

    protected override void Awake()
    {
        base.Awake();
        _resourceLoader = new ResourceLoader();
        _sceneLoader = new SceneLoader();
        _sceneData = new SceneData();
        _scenePlayer = new ScenePlayerManager(_resourceLoader);
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

    private async void SpawnCharacter (CharacterAnimationSequenceData animData)
    {
        var charData = _characterDefinition.Definitions.FirstOrDefault(d=>d.Name == animData.Name);
        var character =  await _resourceLoader.InstantiateAsyncGameObject<CharacterAnimationController>(charData.Asset, animData.Position, animData.Rotation, null);
        var anim = character.GetComponent<Animator>();
        anim.enabled = false;
        anim.runtimeAnimatorController = charData.Animator;
        character.SetData(animData);
        _scenePlayer.RegisterActor(character);
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


    void ICharacterAvatarEventHandler.RespondToCharacterAvatarPressed(string name)
    {
        _currentCharacterData = _characterDefinition.Definitions.FirstOrDefault(d=>d.Name == name);
        _sequenceData = new CharacterAnimationSequenceData(name);
        RefreshAnimationView();
        _sceneBuilderView.HighlightSelectedCharacter(name);
    }

    void IAnimationTimelineEventHandler.RespondToAddAnimation()
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
    

    void IAnimationSelectionEventHandler.RespondToAnimationChanged(int index, string name, ref AnimationSelectionRenderData renderData)
    {
        _sequenceData.UpdateAnimationName(index, name);
        renderData.Name = name;
        renderData.Length = _currentCharacterData.GetAnimationLength(name);
    }

    void IAnimationSelectionEventHandler.RespondToAnimationDurationChanged(int index, float newDuration)
    {
        _sequenceData.UpdateAnimationDuration(index, newDuration);
        if (newDuration > _maxAnimationDuration)
        {
            _maxAnimationDuration = Mathf.CeilToInt(newDuration) + 10;
            RefreshAnimationView();
        }
    }

    void IAnimationSelectionEventHandler.RespondToAnimationRemoved(int index)
    {
        _sequenceData.RemoveAnimation(index);
    }

    async void ISceneBuilderEventHandler.RespondToPlaceButtonPressed()
    {
        if (_currentCharacterData == null)
            return;
        
        _sceneBuilderView.Hide();
        _sceneBuilderView.EnableDisableScenePlayerButton(false);
        var miniChar = await _resourceLoader.InstantiateAsyncGameObject<FollowMouseComponent>(_currentCharacterData.AssetMini, Vector3.zero, Quaternion.identity, null);
        miniChar.Initialize(_resourceLoader, RespondToPlacingCharacter, RespondToCancelPlacingCharacter);
        miniChar.enabled = true;
    }

    void RespondToPlacingCharacter(Vector3 position, Quaternion rotation)
    {
        _sceneBuilderView.EnableDisableScenePlayerButton(true);
        _sequenceData.Position = position;
        _sequenceData.Rotation = rotation;
        _sceneData.AddActor(_sequenceData);
        SpawnCharacter(_sequenceData);
    }

    void RespondToCancelPlacingCharacter()
    {
        _sceneBuilderView.EnableDisableScenePlayerButton(true);
    }

    void ISceneBuilderEventHandler.RespondToPlayButtonPressed()
    {
        _scenePlayer.Play();
    }
    void ISceneBuilderEventHandler.RespondToResetButtonPressed()
    {
        _scenePlayer.Stop();
    }
    
    void ISceneBuilderEventHandler.RespondToSaveButtonPressed()
    {
        _sceneLoader.SaveScene(_sceneData);
    }

    void ISceneBuilderEventHandler.RespondToClearButtonPressed()
    {
        _scenePlayer.Reset();
    }
    void ISceneBuilderEventHandler.RespondToLoadButtonPressed()
    {
        if (!_sceneLoader.HasSavedScene())
            return;

        _scenePlayer.Reset();
        _sceneBuilderView.Hide();
        _sceneData = _sceneLoader.LoadScene();
        foreach (var actor in _sceneData.Actors)
        {
            SpawnCharacter(actor);
        }   
    }

}
