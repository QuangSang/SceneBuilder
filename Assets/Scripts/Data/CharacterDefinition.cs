using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "CharacterDefinition", menuName = "CharacterDefinition")]
public class CharacterDefinition : ScriptableObject
{
    public List<CharacterData> Definitions;
}

[Serializable]
public class CharacterData
{
    public string Name;
    public AssetReferenceGameObject Asset;
    public AssetReferenceGameObject AssetMini;
    public RuntimeAnimatorController Animator;

    public List<string> GetAnimationNames()
    {
        var animNames = new List<string>();
        foreach (var clip in Animator.animationClips)
        {
            animNames.Add(clip.name);
        }
        return animNames;
    }

    public float GetAnimationLength(string name)
    {
        foreach (var clip in Animator.animationClips)
        {
            if (clip.name == name)
            {
                return clip.length;
            }
        }
        return 0f;
    }
}
