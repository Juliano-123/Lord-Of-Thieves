using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
   
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    Apuntar _rotatePointApuntar;

    private void Awake()
    {
        
        _rotatePointApuntar = transform.GetComponentInParent<Apuntar>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SliceEnemy()
    {
        Collider2D[] HitColliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale / 2f, transform.rotation.z);
        
        int i = 0;
        while (i < HitColliders.Length)
        {
            Debug.Log(HitColliders[i].gameObject.tag);

            if (HitColliders[i].gameObject.CompareTag("Enemigo"))
            {
                Destroy(HitColliders[i].gameObject);
                Debug.Log("matado" + HitColliders[i].gameObject.tag);
            }
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
