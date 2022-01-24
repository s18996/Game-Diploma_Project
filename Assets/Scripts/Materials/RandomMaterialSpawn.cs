using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialSpawn : MonoBehaviour {

    [SerializeField] GameObject[] materialObjects;
    [SerializeField] GameObject[] materialPickups;
    GridManager grid;

    private void Awake() {
        grid = GetComponentInParent<GridManager>();
    }

    private void Start() {
        ChooseMaterialToSpawn();
    }

    private void ChooseMaterialToSpawn() {
        if (PrefsManager.IsCrystalOreUpgraded()) {
            int random = Random.Range(0, 100);
            if (random < 70) {
                SpawnMaterial(0);
            }
            else {
                SpawnMaterial(1);
            }
        }
        else {
            SpawnMaterial(0);
        }
    }

    private void SpawnMaterial(int index) {
        GameObject materialObject = Instantiate(materialObjects[index], transform.position, Quaternion.identity, transform.parent);
        grid.materials.Add(materialObject);
        Destroy(gameObject);
    }
}
