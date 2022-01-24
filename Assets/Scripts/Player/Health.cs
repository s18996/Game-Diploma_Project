using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;
    public Image[] hearts;

    public float blink;
    public float immuned;
    private float immunedTime;
    private float blinkTime = 0.1f;
    [SerializeField] private Renderer[] playerModels;

    private void Start() {
        if (PrefsManager.IsHealthUpgraded()) {
            maxHealth++;
        }
        currentHealth = maxHealth;
        for (int i = 0; i < hearts.Length; i++) {
            if (i < currentHealth) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }
        }
    }

    private void Update() {
        if(immunedTime > 0) {
            immunedTime -= Time.deltaTime;
            blinkTime -= Time.deltaTime;

            if(blinkTime <= 0) {
                SetPlayerModelActive(!playerModels[0].enabled);
                blinkTime = blink;
            }
            if(immunedTime <= 0) {
                SetPlayerModelActive(true);
            }
        }
    }

    private void SetPlayerModelActive(bool v) {
        foreach (var renderer in playerModels) {
            renderer.enabled = v;
        }
    }

    public void Damage(float damage) {
        if (immunedTime <= 0) {
            currentHealth -= damage;
            for (int i = 0; i < hearts.Length; i++) {
                if (i < currentHealth) {
                    hearts[i].enabled = true;
                }
                else {
                    hearts[i].enabled = false;
                }
            }
            // Change to end scene
            if (currentHealth <= 0) {
                GameManager.endPanel.SetActive(true);
                GameManager.endPanel.GetComponent<EndPanel>().Show();
                gameObject.SetActive(false);
            }
            else {
                immunedTime = immuned;
                SetPlayerModelActive(true);
                blinkTime = blink;
            }
        }
    }

    public void Heal(float heal) {
        currentHealth += heal;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        for (int i = 0; i < maxHealth; i++) {
            if (i < currentHealth) {
                hearts[i].enabled = true;
            }
        }
    }


}
