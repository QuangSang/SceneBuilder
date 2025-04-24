using System.Collections.Generic;


/// <summary>
/// Manager class responsible for playing the scene
/// </summary>
public class ScenePlayerManager
{
    private List<CharacterAnimationController> _actorList = new();
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    private ResourceLoader _resourceLoader;
    public ScenePlayerManager(ResourceLoader resourceLoader)
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
