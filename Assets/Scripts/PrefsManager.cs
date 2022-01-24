using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsManager : MonoBehaviour {

    public static void SavePlayerSkulls(GameObject player) {
        int allSkulls = PlayerPrefs.GetInt("PlayerSkulls") + player.GetComponent<PlayerController>().GetSkulls();
        PlayerPrefs.SetInt("PlayerSkulls", allSkulls);
    }

    public static void SavePlayerSkulls(int skullsCount) {
        PlayerPrefs.SetInt("PlayerSkulls", skullsCount);
    }

    public static void SetCrystalOreUpgrade(bool value) {
        int intValue = value ? 1 : 0; 
        PlayerPrefs.SetInt("CrystalOre", intValue);
    }

    public static bool IsCrystalOreUpgraded() {
        return PlayerPrefs.GetInt("CrystalOre") != 0;
    }
    public static void SetHealthUpgrade(bool value) {
        int intValue = value ? 1 : 0;
        PlayerPrefs.SetInt("HealthUpgrade", intValue);
    }

    public static bool IsHealthUpgraded() {
        return PlayerPrefs.GetInt("HealthUpgrade") != 0;
    }

}
