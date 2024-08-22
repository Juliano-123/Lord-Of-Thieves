using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Moon : MonoBehaviour, IGolpeable
{
    int _maxHealth = 3;

    [SerializeField]
    GameObject[] RocasChilds;

    BoxCollider2D _boxCollider;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Golpear()
    {
        _maxHealth -= 1;
        _boxCollider.enabled = false;
        Invoke(nameof(ResetearCollider), 2f);

        foreach (GameObject Roca in RocasChilds)
        {
            Roca.GetComponent<IReseteable>().Resetear();

        }

        if (_maxHealth == 0)
        {
            Destroy(gameObject);        
        }

    }


    void ResetearCollider()
    {
        _boxCollider.enabled = true;
    }
}
