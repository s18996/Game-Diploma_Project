using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpriteChanger : MonoBehaviour {

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite shootSprite;

    public void LeftSpawn() {
        spriteRenderer.sprite = leftSprite;
    }
    public void RightSpawn() {
        spriteRenderer.sprite = rightSprite;
    }
    public void DefaultSprite() {
        spriteRenderer.sprite = defaultSprite;
    }
    public void ShootSprite() {
        spriteRenderer.sprite = shootSprite;
    }
}
