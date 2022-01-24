using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] string gameStartScene;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject upgradeMenu;
    UpgradeMenuController upgradeMenuController;

    private void Awake() {
        upgradeMenuController = GetComponent<UpgradeMenuController>();
    }

    private void Start() {
        ActivateMainMenu();
        DisableUpgradeMenu();
    }

    public void ActivateMainMenu() {
        mainMenu.SetActive(true);
    }

    public void DisableMainMenu() {
        mainMenu.SetActive(false);
    }

    public void ActivateUpgradeMenu() {
        upgradeMenu.SetActive(true);
        upgradeMenuController.UpdateUpgradeMenu();
    }

    public void DisableUpgradeMenu() {
        upgradeMenu.SetActive(false);
    }

    public void StartGame() {
        SceneManager.LoadScene(gameStartScene);
    }

    public void OpenUpgradeMenu() {
        DisableMainMenu();
        ActivateUpgradeMenu();
    }

    public void ExitGame() {
        Application.Quit();
        print("Exiting game");
    }

    public void GoToMainMenuFromUpgrades() {
        DisableUpgradeMenu();
        ActivateMainMenu();
    }

}
