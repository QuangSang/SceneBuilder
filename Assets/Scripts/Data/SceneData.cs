using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data class that represent a scene. Can be extended to include props, building, ...
/// </summary>
[Serializable]
public class SceneData
{
    public List<CharacterAnimationSequenceData> Actors = new ();

    public void AddActor(CharacterAnimationSequenceData actorData)
    {
        Actors.Add(actorData);
    }

    public void Print()
    {
        foreach (var actor in Actors)
        {
            actor.Print();
        }
    }
}

[Serializable]
public class CharacterAnimationSequenceData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public List<AnimationData> Sequence;

    public CharacterAnimationSequenceData(string name)
    {
        Name = name;
        Sequence = new();
    }

    public void AddAnimation(AnimationData animData)
    {
        Sequence.Add(animData);
    }

    public void UpdateAnimationName(int index, string newName)
    {
        Sequence[index].AnimationName = newName;
    }

    public void UpdateAnimationDuration(int index, float newDuratiion)
    {
        Sequence[index].Time = newDuratiion;
    }

    public void RemoveAnimation(int index)
    {
        Sequence.RemoveAt(index);
    }

    public void Print()
    {
        Debug.LogError(Name);
        foreach (var anim in Sequence)
        {
            Debug.LogError(anim.AnimationName +" "+anim.Time);
        }
    }
}

[Serializable]
public class AnimationData
{
    public float Time;
    public string AnimationName;
    public bool Loop;
}
