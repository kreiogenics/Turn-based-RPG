using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldIdle : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] int imageNumber;
    [SerializeField] Sprite[] sprite;

    private void Start() 
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();   
    }
    void Update()
    {
        switch (imageNumber)
        {
            case 0:
                ChangeSprite(0);
                break;
            case 1:
                ChangeSprite(1);
                break;
            default:
                break;
        }
        
    }

    private void ChangeSprite(int i)
    {
        spriteRenderer.sprite = sprite[i];
    }
}
