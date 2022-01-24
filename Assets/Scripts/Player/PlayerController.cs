using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GridManager grid = null;

    private Health health = null;
    private PlayerCombat combatScript = null;
    private PlayerMining miningScript = null;
    private InventoryManager inventoryManager = null;

    public int skullsCount = 0;
    public int ironCount = 0;
    public int copperCount = 0;
    public int crystalCount = 0;

    public float additiveCopperDamage;
    public float additiveCrystalDamage;
    public bool isUsingCopper;
    public bool isUsingCrystal;

    //private int goldCount = 0;

    private void Awake() {
        health = GetComponent<Health>();
        combatScript = GetComponent<PlayerCombat>();
        miningScript = GetComponent<PlayerMining>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start() {
        miningScript.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.isUpgradeMenuOpen) {
            GameManager.OpenPauseMenu();
        }
    }

    public void AddSkull() {
        skullsCount++;
        inventoryManager.SetSkullText(GetSkulls());
    }

    public void SetSkulls(int count) {
        skullsCount = count;
        if (skullsCount < 0)
            skullsCount = 0;
        inventoryManager.SetSkullText(GetSkulls());
    }

    public void AddCrystal() {
        crystalCount++;
        inventoryManager.SetCrystalText(GetCrystals());
    }
    public void SetCrystals(int count) {
        crystalCount = count;
        if (crystalCount < 0)
            crystalCount = 0;
        inventoryManager.SetCrystalText(GetCrystals());
    }

    public int GetCrystals() {
        return crystalCount;
    }

    public int GetSkulls() {
        return skullsCount;
    }

    public void AddCopper() {
        copperCount++;
        inventoryManager.SetCopperText(GetCoppers());
    }

    public void SetCoppers(int count) {
        copperCount = count;
        if (copperCount < 0)
            copperCount = 0;
        inventoryManager.SetCopperText(GetCoppers());
    }

    public int GetCoppers() {
        return copperCount;
    }

    public void UseSword() {
        combatScript.enabled = true;
        combatScript.SwordActivation(true);
        miningScript.PickaxeActivation(false);
        miningScript.enabled = false;
    }

    public void UsePickaxe() {
        miningScript.enabled = true;
        miningScript.PickaxeActivation(true);
        combatScript.SwordActivation(false);
        combatScript.enabled = false;
    }

    public void Heal(float heal) {
        health.Heal(heal);
    }

    public void UsingCopper() {
        isUsingCopper = true;
        isUsingCrystal = false;
    }

    public void UsingCrystal() {
        isUsingCopper = false;
        isUsingCrystal = true;
    }

}

