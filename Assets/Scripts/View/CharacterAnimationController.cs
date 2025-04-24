using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// a Component that is responsible for playing the character animtions
/// </summary>
public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _currentTime;
    [SerializeField] private int _currentIndex;
    private CharacterAnimationSequenceData _data;
    private Vector3 _originPosition;
    private Quaternion _originRotation;
    private List<AnimationData> _sequence;
    private string _previousAnimation;

    void Awake()
    {
        _currentTime = -1f;
        _currentIndex = -1;
    }

    public void SetData(CharacterAnimationSequenceData data)
    {
        _data = data;
        _originPosition = transform.position;
        _originRotation = transform.rotation;
    }

    public void Play()
    {
        if (_data == null)
            return; 
        if (_data.Sequence.Count <= 0)
            return;

        _sequence = new List<AnimationData>(_data.Sequence);
        _sequence.Sort((a,b)=>a.Time.CompareTo(b.Time));
        _currentTime = 0f;
        _currentIndex = 0;
    }

    public void Stop()
    {
        transform.position = _originPosition;
        transform.rotation = _originRotation;
        _animator.StopPlayback();
        _animator.enabled = false;
        _currentTime = -1f;
        _currentIndex= -1;
        _animator.Rebind();
        _animator.Update(0f);
    }

    void Update()
    {
        if (_currentTime < 0f)
            return;
        
        _currentTime += Time.deltaTime;
        if (_currentTime > _sequence[_currentIndex].Time)
        {
            PlayAnimation(_sequence[_currentIndex].AnimationName);
            _currentIndex++;
        }
        if (_currentIndex >= _sequence.Count)
        {
            _currentTime = -1f;
        }
    }

    private void PlayAnimation(string animName)
    {
        _animator.enabled = true;
        if (_previousAnimation == animName)
        {
            _animator.Rebind();
            _animator.Update(0f);
        }else{
            _animator.CrossFade(animName, 0.1f);
        }
        _previousAnimation = animName;
    }

}
