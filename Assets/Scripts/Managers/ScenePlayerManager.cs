using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class ScenePlayerManager : Singleton<ScenePlayerManager>
{
    private List<CharacterAnimationController> _actorList = new();
    private bool _isPlaying = false;
    private ResourceLoader _resourceLoader;
    public void Initialize(ResourceLoader resourceLoader)
    {
        _resourceLoader = resourceLoader;
    }
    public void RegisterActor(CharacterAnimationController charController)
    {
        _actorList.Add(charController);
    }

    public void Play()
    {
        if (_isPlaying)
        {
            Stop();
            return;
        }

        _isPlaying = true;
        foreach (var actor in _actorList)
        {
            actor.Play();
        }
    }

    public void Stop()
    {
        _isPlaying = false;
        foreach (var actor in _actorList)
        {
            actor.Stop();
        }
    }

    public void Reset()
    {
        Stop();
        foreach (var actor in _actorList)
        {
            _resourceLoader.ReleaseGO(actor.gameObject);
        }
        _actorList.Clear();
    }
}
