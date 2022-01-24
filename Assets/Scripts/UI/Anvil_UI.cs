using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Anvil_UI : MonoBehaviour {

    //private static string filePath = "Assets/Graphics/Sprites/Player/MySword.png";
    private static string filePath = "MySword.png";
    private static string defaultText = "Available: ";
    private static string defaultDamageText = "Damage: ";

    public int playerCoppers;
    public int playerCrystals;
    public UpgradeCell[] cells;

    private GameObject UpgradeUI;
    private GameObject cellsParent;
    private GameObject crystalObject;
    private Text copperText;
    private Text crystalText;
    private Text damageText;
    private PlayerController player;
    private PlayerCombat playerCombat;

    private bool playerInRange;

    private void Awake() {
        UpgradeUI = GameManager.upgradeWindow;
        cellsParent = GameManager.upgradeWindow.transform.Find("Upgrade").gameObject;
        copperText = GameManager.upgradeWindow.transform.Find("Resources").transform.Find("Copper").transform.Find("CopperCount").GetComponent<Text>();
        copperText.text = defaultText;
        crystalObject = GameManager.upgradeWindow.transform.Find("Resources").transform.Find("Crystal").gameObject;
        damageText = GameManager.upgradeWindow.transform.Find("Menu").transform.Find("DamageText").GetComponent<Text>();
        damageText.text = defaultDamageText;
        player = GameManager.player.GetComponent<PlayerController>();
        playerCombat = GameManager.player.GetComponent<PlayerCombat>();

        if (PrefsManager.IsCrystalOreUpgraded()) {
            crystalObject.SetActive(true);
            crystalText = crystalObject.transform.Find("CrystalCount").GetComponent<Text>();
            crystalText.text = defaultText;
        }
        else {
            crystalObject.SetActive(false);
        }

    }

    private void Start() {
        cells = cellsParent.transform.GetComponentsInChildren<UpgradeCell>();
        foreach (UpgradeCell cell in cells) {
            cell.SetAnvil(this);
        }
    }

    private void Update() {
        if (playerInRange && Input.GetMouseButtonDown(1)) {
            OpenAnvil();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseAnvil();
        }
    }

    private void OpenAnvil() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Anvil")) {
            SetSwordCells();
            playerCoppers = player.GetCoppers();
            playerCrystals = player.GetCrystals();
            UpdateTexts();
            UpgradeUI.SetActive(true);
            GameManager.isUpgradeMenuOpen = true;
        }
    }

    private void CloseAnvil() {
        UpgradeUI.SetActive(false);
        GameManager.isUpgradeMenuOpen = false;
    }

    public void UpdateTexts() {
        SetText(copperText, player.GetCoppers().ToString(), defaultText);
        if (PrefsManager.IsCrystalOreUpgraded()) {
            SetText(crystalText, player.GetCrystals().ToString(), defaultText);
        }
        SetText(damageText, playerCombat.attackDamage.ToString(), defaultDamageText);
    }

    private void SetText(Text text, string addedText, string defaultString) {
        text.text = defaultString;
        text.text += addedText;
    }

    private void SetSwordCells() {
        for (int i = 0; i < 32; i++) {
            for (int j = 0; j < 32; j++) {
                Sprite sword = GameObject.Find("Player").transform.Find("Sword").transform.Find("Sword_base").GetComponent<SpriteRenderer>().sprite;
                Color color = sword.texture.GetPixel(31 - j, 31 - i);
                if (color.a != 0.0f) {
                    cells[i * 32 + j].SetColor(sword.texture.GetPixel(31 - j, 31 - i));
                }
                else {
                    cells[i * 32 + j].SetColor(Color.white);
                }
                cells[i * 32 + j].row = i;
                cells[i * 32 + j].col = j;
            }
        }
    }

    private void SaveToFile(Texture2D texture, string filePath) {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }

    private void LoadTexture(string filePath) {
        Texture2D texture;
        byte[] FileData;

        if (File.Exists(filePath)) {
            FileData = File.ReadAllBytes(filePath);
            texture = new Texture2D(32, 32);
            if (texture.LoadImage(FileData)) {
                SpriteRenderer swordBase = GameObject.Find("Player").transform.Find("Sword").transform.Find("Sword_base").GetComponent<SpriteRenderer>();
                swordBase.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 32f);
            }
        }
    }

    public void SaveUpgrade() {
        Texture2D newTexture = GetNewTexture();
        SaveToFile(newTexture, filePath);
        LoadTexture(filePath);
    }

    private Texture2D GetNewTexture() {
        Texture2D newTexture = new Texture2D(32, 32);
        cellsParent = GameObject.Find("Upgrade");
        cells = cellsParent.transform.GetComponentsInChildren<UpgradeCell>();
        for (int i = 0; i < 32; i++) {
            for (int j = 0; j < 32; j++) {
                Color color = cells[i * 32 + j].GetColor();
                if (!color.Equals(Color.white))
                    newTexture.SetPixel(31 - j, 31 - i, cells[i * 32 + j].GetColor());
                else {
                    newTexture.SetPixel(31 - j, 31 - i, new Color(0f, 0f, 0f, 0f));
                }
            }
        }
        return newTexture;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerInRange = false;
        }
    }

}