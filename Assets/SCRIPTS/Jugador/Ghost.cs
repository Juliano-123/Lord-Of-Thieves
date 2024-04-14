using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    float ghostDelay = 0.1f;
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
                Sprite _currentPlayerSprite = GetComponent<SpriteRenderer>().sprite;
                SpriteRenderer _ghostSpriteRenderer = currentGhost.GetComponent<SpriteRenderer>();
                _ghostSpriteRenderer.sprite = _currentPlayerSprite;
                //VOLTEA EL SPRITE
                if (Player.orientacionX == 1)
                {
                    _ghostSpriteRenderer.flipX = false;
                }
                else if (Player.orientacionX == -1)
                {
                    _ghostSpriteRenderer.flipX = true;
                }
                currentGhost.transform.localScale = transform.localScale;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 0.29f);
            }

        }

    }
}
