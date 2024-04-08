using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost = false;

    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    void Update()
    {
        if (makeGhost == true)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                currentGhost.transform.localScale = transform.localScale;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 0.29f);
            }
        }

    }
}
