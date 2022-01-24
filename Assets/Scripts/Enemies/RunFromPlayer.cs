using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunFromPlayer : MonoBehaviour {

    [SerializeField] private float runningSpeed = 0;
    GameObject targetPlayer = null;
    GridManager grid;
    Rigidbody2D _rigidbody;
    bool playerInRange;

    void Awake() {
        grid = GameObject.Find("Grid").GetComponent<GridManager>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (playerInRange) {
            RunAway();
        }
    }

    private void RunAway() {
        float distance = Vector2.Distance(transform.position, targetPlayer.transform.position);
        float middleDistance = Vector2.Distance(transform.position, new Vector3(7, -4, 0));
        if (middleDistance > 5) {
            Vector3 offset = transform.position - targetPlayer.transform.position;
            offset *= 2;
            Vector3 middle = new Vector3(7, -4, 0);
            Vector3 newPos = transform.position + offset + middle;
            newPos /= 2;
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, newPos, runningSpeed / distance * Time.fixedDeltaTime));
        }
        else {
            Vector3 targetPosition = transform.position - targetPlayer.transform.position;
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, transform.position + targetPosition, runningSpeed / (distance / 2f) * Time.fixedDeltaTime));
        }


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
