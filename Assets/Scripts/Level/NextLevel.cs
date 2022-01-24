using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour {

    [SerializeField] GameObject nextLevel;

    public bool isLastLevel;

    GameManager gameManager;
    GameObject player;

    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void GoToNextLevel() {
        PrefsManager.SavePlayerSkulls(player);
        if (isLastLevel) {
            GameManager.OpenCredits();
        }
        else {
            //Instantiate(nextLevel);
            nextLevel.SetActive(true);
            player.transform.position = new Vector2(7, -4);
            gameManager.SetCurrentLevel(nextLevel.GetComponent<LevelManager>());
            //GameManager.minimap.GetComponent<MinimapController>().ClearRooms();
            gameObject.SetActive(false);
        }
    }
}
