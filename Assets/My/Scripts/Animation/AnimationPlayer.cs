using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    System.Action _beginCallBack = null;
    System.Action _midCallBack = null;
    System.Action _endCallBack = null;
    Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play(string trigger,
        System.Action beginCallback=null,
        System.Action midCallback=null,
        System.Action endCallback = null)
    {
        animator.SetTrigger(trigger);
        _beginCallBack = beginCallback;
        _midCallBack = midCallback;
        _endCallBack = endCallback;
    }

    #region AnimationEvent
    public void OnBeginEvent()
    {
        _beginCallBack?.Invoke();
    }

    public void OnMidEvent()
    {
        _midCallBack?.Invoke();
    }

    public void OnEndEvent()
    {
        _endCallBack?.Invoke();
    }

    void InitializedDeligate()
    {
        _beginCallBack = null;
        _midCallBack = null;
        _endCallBack = null;
    }

    private void OnDestroy()
    {
        InitializedDeligate();
    }
    #endregion
}
