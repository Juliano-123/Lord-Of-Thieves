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


            ResetCameraConfiner();

    }



    void ResetCameraConfiner()
    {
        GetComponent<CinemachineConfiner2D>().InvalidateCache();   
    }


}
