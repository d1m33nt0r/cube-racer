using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBalloonsCounter : MonoBehaviour
{
    private int count = 0;
    public bool unlocked;

    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AddOne()
    {
        count++;
        
        if (count == 3 && !unlocked)
            _animator.Play("ShowUI");
        
        if (count == 9 && unlocked)
            _animator.Play("ShowNextLevelButton");
    }

    public void HideButtons()
    {
        unlocked = true;
        _animator.Play("HideButtons");
    }
    
    public int GetCount()
    {
        return count;
    }
    
    public void ResetCounter()
    {
        count = 0;
    }
}
