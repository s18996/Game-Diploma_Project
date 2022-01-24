using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] HealthBar healthBar = null;
    [SerializeField] float maxHealth = 4;
    [SerializeField] GameObject loot = null; 
    [SerializeField] bool isBoss = false;
    [SerializeField] bool isBossMinion = false;
    public float damage = 1f;


    [HideInInspector] public EnemiesCounter counter = null; 

    public float currentHealth = 0f;

    private void Awake() {
        counter = gameObject.GetComponentInParent<EnemiesCounter>();
    }

    private void Start() {
        SetMaxHealth();
        if (!isBoss) {
            healthBar.gameObject.SetActive(false);
        }
        else {
            healthBar.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(float damage) {
        healthBar.gameObject.SetActive(true);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth/maxHealth);
        if(currentHealth <= 0) {
            if (isBoss) {
                counter.BossDead();
            }
            if (loot != null && !isBossMinion) {
                SpawnLoot();
            }
            if (counter != null) {
                counter.EnemyDead();
            }
            if (isBossMinion) {
                gameObject.SetActive(false);
            }
            else {
                Destroy(gameObject);
            }
        }
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetHealth() {
        return currentHealth;
    }

    public void SetMaxHealth() {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth / maxHealth);
    }

    public void Heal(float value) {
        print("Heal: " + value);
        currentHealth += value;
        print("After heal: " + currentHealth);
        healthBar.SetHealth(currentHealth / maxHealth);
    }
    private void SpawnLoot() {
        Instantiate(loot, transform.position, Quaternion.identity, transform.parent);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Sword")) {
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Sword")) {
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }
    }
}
