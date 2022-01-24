using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnterHandler : MonoBehaviour {

    [SerializeField] Vector2Int direction = new Vector2Int(0, 0);
    [SerializeField] GameManager gameManager;
    
    private GameObject player = null;

    private void Awake() {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            LevelManager currentLevel = gameManager.GetCurrentLevel();
            currentLevel.SetCurrentRoom(currentLevel.currentRoom.GetPosition() + direction);
            player.transform.position = PlayerDestination(direction);
        }
    }

    private Vector2 PlayerDestination(Vector2Int direction) {
        if (direction == Vector2Int.up)
            return new Vector2(7, -7.5f);
        else if (direction == Vector2Int.right)
            return new Vector2(0.5f, -4);
        else if (direction == Vector2Int.down)
            return new Vector2(7, -0.5f);
        else if (direction == Vector2Int.left)
            return new Vector2(13.5f, -4);
        else
            return Vector2.zero;
    }

}
