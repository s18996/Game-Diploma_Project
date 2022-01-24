using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    
    [SerializeField] PlayerCombat player = null;
    private float damage = 1;

    public float GetDamage() {
        return damage;
    }

    public void SetDamage(float damage) {
        this.damage = damage;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage * player.attackDamage);
        }
    }
}
