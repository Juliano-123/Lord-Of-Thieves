using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gemasTimerBar : MonoBehaviour
{
    Image gemasTimerBarImage;

    void Start()
    {
        gemasTimerBarImage = GetComponent<Image>();        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Player.gemasContadas != 0)
        //{
        //    gemasTimerBarImage.fillAmount = 1 - (Player.timergemascontadas / Player.timeGemasDecay);
        //}
    }
}
