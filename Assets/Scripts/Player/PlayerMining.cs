using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour {

    [SerializeField] GameObject pickaxe = null;
    [SerializeField] float attackRate = 4f;
    private float nextAttackTime = 0f;

    private GridManager grid = null;
    private PlayerController player = null;
    private Vector3 mousePos;
    private bool swinging = false;
    private bool isReturning = false;

    void Awake() {
        player = GetComponent<PlayerController>();
        grid = player.grid;
    }

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        grid.SetBorder(mousePos);
        if (Time.time >= nextAttackTime) {
            if (Input.GetMouseButtonDown(0)) {
                MiningMaterial();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void MiningMaterial() {
        SwingPickaxe();
        grid = GetComponent<PlayerController>().grid;
        Field hoveredField = grid.GetField(mousePos);
        if (hoveredField != null && hoveredField.objectType == ObjectType.Destructible && IsFieldCloseToPlayer(hoveredField)) {
            DestroyMaterial();
            hoveredField.RestartFieldType();
        }
    }
    void SwingPickaxe() {
        isReturning = true;
        if (!swinging) {
            StartCoroutine(PickaxeRotation(90f, 0.08f));
        }
    }

    IEnumerator PickaxeRotation(float angle, float duration) {
        swinging = true;
        Quaternion startingRotation = pickaxe.transform.rotation;
        Quaternion endingRotation;

        if (mousePos.x < transform.position.x) {
            endingRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else {
            endingRotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        }
        for (float t = 0; t < duration; t += Time.deltaTime) {
            pickaxe.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, t / duration);
            yield return null;
        }
        pickaxe.transform.rotation = endingRotation;
        swinging = false;
        if (isReturning) {
            isReturning = false;
            StartCoroutine(PickaxeBackRotation(-90f, 0.17f));
        }
    }
    IEnumerator PickaxeBackRotation(float angle, float duration) {
        swinging = true;
        Quaternion startingRotation = pickaxe.transform.rotation;
        Quaternion endingRotation = Quaternion.identity;

        for (float t = 0; t < duration; t += Time.deltaTime) {
            pickaxe.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, t / duration);
            yield return null;
        }
        pickaxe.transform.rotation = endingRotation;
        swinging = false;
    }

    public void PickaxeActivation(bool activation) {
        if (pickaxe != null)
            pickaxe.SetActive(activation);
    }

    bool IsFieldCloseToPlayer(Field field) {
        foreach (Field fieldToCheck in grid.GetNeighbourFields(GetPlayerFieldOnGrid())) {
            if (fieldToCheck != null && field == fieldToCheck)
                return true;
        }
        return false;
    }

    void DestroyMaterial() {
        GameObject material = grid.GetMaterialObject(mousePos);
        material.GetComponent<Destructible>().SpawnLoot();
        grid.materials.Remove(material);
        Destroy(material); //- najpierw usuwaæ z listy w GRID grid.GetMaterialObject()
    }
    public Field GetPlayerFieldOnGrid() {
        return grid.GetField(transform.position);
    }

}
