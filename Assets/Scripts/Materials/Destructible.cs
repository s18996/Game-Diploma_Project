using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public GameObject material = null;
    public Collectible collectible = null;
    GridManager grid;

    private void Awake() {
        grid = GetComponentInParent<GridManager>();
        collectible = material.GetComponent<Collectible>();
    }

    private void Start() {
        grid.SetDestructible(new Vector2Int((int)transform.position.x, -(int)transform.position.y));
    }

    public void SpawnLoot() {
        if (collectible.pickupType == PickupType.Crystal) {
            int random = Random.Range(1, 3);
            for (int i = 0; i < random; i++) {
                Instantiate(material, transform.position, Quaternion.identity, transform.parent);
            }
        }
        else {
            Instantiate(material, transform.position, Quaternion.identity, transform.parent);
        }
    }
}
