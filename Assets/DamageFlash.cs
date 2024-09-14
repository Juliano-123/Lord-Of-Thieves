using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField]
    Color _flashColor = Color.white;
    [SerializeField]
    float _flashTime = 0.25f;

    [SerializeField]
    SpriteRenderer _spriteRenderer;
    Material _material;


    private void Awake()
    {
        _material = _spriteRenderer.material;
    }

    IEnumerator DamageFlasher()
    {
        _material.SetColor("_FlashColor", _flashColor);

        float currentFlashAmount = 0;
        float elapsedTime = 0;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0, (elapsedTime / _flashTime));
            _material.SetFloat("_FlashAmount", currentFlashAmount);
            yield return null;
        }

        yield break;
    }

    public void CallDamageFlash()
    {
        StartCoroutine(DamageFlasher());
        Debug.Log("damageflasher llamado");
    }

}
