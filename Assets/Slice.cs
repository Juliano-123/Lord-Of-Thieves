using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SliceEnemy()
    {

    }

    public void EndSlice()
    { 
        _spriteRenderer.enabled = false;
        _animator.enabled = false;
    }

}
