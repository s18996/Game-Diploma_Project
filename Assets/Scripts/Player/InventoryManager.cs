using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    [SerializeField] Color enabledColor;
    [SerializeField] Color disabledColor;
    [SerializeField] Image swordSlot = null;
    [SerializeField] Image pickaxeSlot = null;
    [SerializeField] TextMeshProUGUI copperCount = null;
    [SerializeField] TextMeshProUGUI crystalCount = null;
    [SerializeField] TextMeshProUGUI skullCount = null;

    private PlayerController player = null;

    void Awake() {
        player = GetComponent<PlayerController>();
        player.UseSword();
        if (PrefsManager.IsCrystalOreUpgraded()) {
            crystalCount.transform.parent.gameObject.SetActive(true);
        }
        else {
            crystalCount.transform.parent.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            PlayerUseSword();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            PlayerUsePickaxe();
        }
    }

    private void PlayerUsePickaxe() {
        pickaxeSlot.color = enabledColor;
        swordSlot.color = disabledColor;
        player.UsePickaxe();
    }

    private void PlayerUseSword() {
        swordSlot.color = enabledColor;
        pickaxeSlot.color = disabledColor;
        player.UseSword();
    }

    public void SetCopperText(int count) {
        copperCount.text = count.ToString();
    }

    public void SetSkullText(int count) {
        skullCount.text = count.ToString();
    }

    internal void SetCrystalText(int count) {
        crystalCount.text = count.ToString();
    }
}
