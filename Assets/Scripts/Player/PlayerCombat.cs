using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public LayerMask enemyLayers;
    public float attackDamage = 1f;
    [SerializeField] float range = 1.5f;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] GameObject sword = null;

    private bool swinging = false;
    private bool isReturning = false;
    private float swordAttackAngle = -120f;

    private Vector3 mousePos;
    private Vector3 fromPlayerToMouse;

    void Update() {
        SetMousePosition();
        Debug.DrawLine(transform.position, transform.position + fromPlayerToMouse, Color.red);

        if (!swinging) {
            RotateSwordToMousePos();
        }
        if(Time.time >= nextAttackTime) {
            if (Input.GetMouseButtonDown(0)) {
                Attack();
                SwingSword();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void SwingSword() {
        isReturning = true;
        if (!swinging) {
            StartCoroutine(SwordRotation(swordAttackAngle, 0.4f / attackRate));
        }
    }

    void RotateSwordToMousePos() {
        float angle = AngleBetweenTwoPoints(transform.position, transform.position + fromPlayerToMouse);
        sword.transform.rotation = Quaternion.Euler(0, 0, angle+180);
    }

    IEnumerator SwordRotation(float angle, float duration) {
        swinging = true;
        Quaternion startingRotation = sword.transform.rotation;
        Quaternion endingRotation = Quaternion.Euler(new Vector3(0, 0, angle)) * startingRotation;
        for (float t = 0; t < duration; t += Time.deltaTime) {
            sword.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, t / duration);
            yield return null;
        }
        sword.transform.rotation = endingRotation;
        swinging = false;
        if (isReturning) {
            isReturning = false;
            StartCoroutine(SwordRotation(-swordAttackAngle, 0.6f / attackRate));
        }
    }

    void SetMousePosition() {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        fromPlayerToMouse = mousePos - transform.position;
        fromPlayerToMouse = fromPlayerToMouse.normalized;
    }

    void Attack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + fromPlayerToMouse, range, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            if (!enemy.isTrigger) {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected() {
        if (fromPlayerToMouse == null)
            return;
        Gizmos.DrawWireSphere(transform.position + fromPlayerToMouse, range);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void SwordActivation(bool activation) {
        if (sword != null)
            sword.SetActive(activation);
    }


}
