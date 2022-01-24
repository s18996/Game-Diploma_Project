using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour {

    [SerializeField] TextMeshProUGUI playerSkullsText;
    [SerializeField] GameObject FirstUpgradeButton;
    [SerializeField] GameObject SecondUpgradeButton;
    private int playerSkulls;

    // Prefs names in GameManager.cs

    public void ResetSkullsAndUpgrades() {
        PrefsManager.SetCrystalOreUpgrade(false);
        PrefsManager.SetHealthUpgrade(false);
        PlayerPrefs.SetInt("PlayerSkulls", 0);
        UpdateUpgradeMenu();
    }

    public void AddSkulls() {
        PlayerPrefs.SetInt("PlayerSkulls", 100);
        UpdateUpgradeMenu();
    }

    public void UpdateUpgradeMenu() {
        UpdatePlayerSkullsText();
        UpdateUpgrades();
    }

    private void UpdateUpgrades() {
        FirstUpgradeButton.SetActive(!PrefsManager.IsCrystalOreUpgraded());
        SecondUpgradeButton.SetActive(!PrefsManager.IsHealthUpgraded());
    }

    private void UpdatePlayerSkullsText() {
        playerSkulls = PlayerPrefs.GetInt("PlayerSkulls");
        playerSkullsText.text = playerSkulls.ToString();
    }

    public void UpgradeCrystalOre() {
        if(playerSkulls >= 20) {
            playerSkulls -= 20;
            PrefsManager.SavePlayerSkulls(playerSkulls);
            PrefsManager.SetCrystalOreUpgrade(true);
            UpdatePlayerSkullsText();
            FirstUpgradeButton.SetActive(false);
            print("Crystal Ore upgrade bought");
        }
        else {
            // unable to buy
            print("not enough SKULLS");
        }
    }

    public void UpgradeMoreHealth() {
        if (playerSkulls >= 40) {
            playerSkulls -= 40;
            PrefsManager.SavePlayerSkulls(playerSkulls);
            PrefsManager.SetHealthUpgrade(true);
            UpdatePlayerSkullsText();
            SecondUpgradeButton.SetActive(false);
            print("Health upgraded");
        }
        else {
            // unable to buy
            print("not enough SKULLS");
        }
    }

}
