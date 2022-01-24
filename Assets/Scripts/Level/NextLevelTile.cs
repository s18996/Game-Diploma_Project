using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTile : MonoBehaviour {

    [SerializeField] Sprite closedTile;
    [SerializeField] Sprite openTile;
    [SerializeField] SpriteRenderer spriteRenderer;
    private NextLevel nextLevel;

    private bool isPlayerReady;

    private void Awake() {
        nextLevel = GetComponentInParent<NextLevel>();
    }

    private void Start() {
        CheckIfPlayerInside();
    }

    private void CheckIfPlayerInside() {
        if(Vector2.Distance(GameManager.player.transform.position, transform.position) < 3f) {
            isPlayerReady = false;
        }
        else {
            isPlayerReady = true;
            ChangeSprite(openTile);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isPlayerReady = true;
            ChangeSprite(openTile);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && isPlayerReady) {
            nextLevel.GoToNextLevel();
        }
    }

    private void ChangeSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

}
