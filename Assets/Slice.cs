using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    Transform _rotatePointTransform;
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    Apuntar _rotatePointApuntar;

    private void Awake()
    {
        _rotatePointTransform = transform.GetComponentInParent<Transform>();
        _rotatePointApuntar = transform.GetComponentInParent<Apuntar>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SliceEnemy()
    {
        Collider2D[] HitColliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale / 2.5f, transform.rotation.z);
        
        int i = 0;
        while (i < HitColliders.Length)
        {
            
            Debug.Log("Hit : " + HitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }

    }

    public void EndSlice()
    { 
        _spriteRenderer.enabled = false;
        _animator.enabled = false;
        _rotatePointApuntar.enabled = true;
    }

}
