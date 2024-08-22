using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class ResetConfiner : MonoBehaviour
{
    public static ResetConfiner Instance;


    [SerializeField]
    SpriteRenderer _spriteRendererCheckDeCamara;

    bool _reset = false;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        ResetCameraConfiner();
    }

    private void Update()
    {

        if (_spriteRendererCheckDeCamara.isVisible == true)
        {
            ResetCameraConfiner();
        }
    }


    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    //static void OnAfterSceneLoad()
    //{
    //    //ResetConfiner _resetConfiner = gameObject.AddComponent(typeof(ResetConfiner)) as ResetConfiner;
    //    //_resetConfiner.ResetCameraConfiner();
    //    //Debug.Log("First scene loaded: After Awake is called.");
    //}

    void ResetCameraConfiner()
    {
        GetComponent<CinemachineConfiner2D>().InvalidateCache();   
    }

    public void ResetConfinerExterno()
    {
        _reset = true;
    }

}
