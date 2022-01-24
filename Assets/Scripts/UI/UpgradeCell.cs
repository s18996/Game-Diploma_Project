using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCell : MonoBehaviour {

    public int row;
    public int col;

    public Image image;
    public Button button;
    public PlayerController playerController;
    public PlayerCombat playerCombat;
    public Anvil_UI anvil;

    void Start() {
        playerController = GameManager.player.GetComponent<PlayerController>();
        playerCombat = GameManager.player.GetComponent<PlayerCombat>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void SetAnvil(Anvil_UI anvil) {
        this.anvil = anvil;
    }

    public void SetColor(Color newColor) {
        image.color = newColor;
    }

    public Color GetColor() {
        return image.color;
    }

    public void ClearCell() {
        image.color = Color.white;
    }

    public void OnClick() {
        //print(row + " : " + col);
        if (playerController.isUsingCopper && playerController.GetCoppers() > 0) {
            playerController.SetCoppers(playerController.GetCoppers() - 1);
            playerCombat.attackDamage += playerController.additiveCopperDamage;
            anvil.UpdateTexts();
            SetColor(new Color(255f / 255f, 112f / 255f, 77f / 255f, 1f));
        }
        else if (playerController.isUsingCrystal && playerController.GetCrystals() > 0) {
            playerController.SetCrystals(playerController.GetCrystals() - 1);
            playerCombat.attackDamage += playerController.additiveCrystalDamage;
            anvil.UpdateTexts();
            SetColor(new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f));
        }
    }

}
