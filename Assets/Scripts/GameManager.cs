using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public LevelManager currentLevel;
    public LevelManager firstLevel;

    public static GameObject endPanel;
    public static GameObject minimap;
    public static GameObject upgradeWindow;
    public static GameObject player;
    public static GameObject pauseMenu;
    public static GameObject credits;
    public static bool isUpgradeMenuOpen;

    private void Awake() {
        currentLevel = firstLevel;
        endPanel = GameObject.Find("EndPanel");
        upgradeWindow = GameObject.Find("Upgrade_window");
        pauseMenu = GameObject.Find("PauseMenu");
        credits = GameObject.Find("Credits");
        minimap = GameObject.Find("Minimap");
        player = GameObject.Find("Player");
        isUpgradeMenuOpen = false;
    }

    private void Start() {
        upgradeWindow.SetActive(false);
    }

    public void SetCurrentLevel(LevelManager level) {
        currentLevel = level;
        //currentLevel.LevelStart();
    }

    public LevelManager GetCurrentLevel() {
        return currentLevel;
    }

    public static void ChangeToMenuScene() {
        PrefsManager.SavePlayerSkulls(player);
        SceneManager.LoadScene("Menu");
    }
    public static void RestartScene() {
        SceneManager.LoadScene("GameScene");
    }

    public static void OpenPauseMenu() {
        pauseMenu.GetComponent<PauseMenu>().Show();
    }

    public static void OpenCredits() {
        credits.GetComponent<Credits>().Show();
    }

}
