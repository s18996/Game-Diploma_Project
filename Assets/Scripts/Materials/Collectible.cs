using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public PickupType pickupType = PickupType.Copper;
    private PlayerController player;
    [SerializeField] Collider2D objectCollider = null;
    private Rigidbody2D rb = null;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        objectCollider.enabled = false;
        AddRandomForce();
        Invoke(nameof(DisableAnimation), 0.5f);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (player == null)
                player = collision.GetComponent<PlayerController>();
            switch (pickupType) {
                case PickupType.Copper:
                    player.AddCopper();
                    Destroy(gameObject);
                    break;
                case PickupType.Crystal:
                    player.AddCrystal();
                    Destroy(gameObject);
                    break;
                case PickupType.Skull:
                    player.AddSkull();
                    Destroy(gameObject);
                    break;
                case PickupType.Heal:
                    player.Heal(1f);
                    Destroy(gameObject);
                    break;
            }
        }
    }
    
    void DisableAnimation() {
        objectCollider.enabled = true;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
    }

    void AddRandomForce() {
        rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * Random.Range(-1f, 1f), ForceMode2D.Impulse);
    }

}
public enum PickupType {
    Copper,
    Crystal,
    Skull,
    Heal
}
