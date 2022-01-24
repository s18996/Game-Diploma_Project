using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAtPlayer : MonoBehaviour {

    [SerializeField] float shotingCooldown = 1f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite shootSprite;
    [SerializeField] Sprite defaultSprite;
    private bool isInShootSprite;
    //[SerializeField] float speedMultiplier = 1f;
    //[SerializeField] float damageMultiplier = 1f;

    private Rigidbody2D player = null;

    private void Awake() {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        isInShootSprite = false;
        InvokeRepeating("Shot", 1f, shotingCooldown);
        InvokeRepeating("ChangeSprite", 1f, shotingCooldown/2f);
    }

    private void Shot() {
        GameObject projectile = ObjectPooling.Instance.SpawnFromPool("Projectile",
            transform.position,
            Quaternion.Euler(new Vector3(0, 0, AngleBetweenTwoPoints(transform.position, player.position) - 90f)));
        projectile.GetComponent<Projectile>().StartMove(transform, player.transform);
    }

    private void ChangeSprite() {
        if (isInShootSprite) {
            spriteRenderer.sprite = defaultSprite;
            isInShootSprite = false;
        }
        else {
            spriteRenderer.sprite = shootSprite;
            isInShootSprite = true;
        }
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}
