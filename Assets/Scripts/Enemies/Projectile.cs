using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 1f;
    public float damage = 1f;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartMove(Transform origin, Transform target) {
        rb.velocity = (target.position - origin.position).normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }
        gameObject.SetActive(false);
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }
}
