using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCounter : MonoBehaviour {

    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject nextLevelTile;
    [SerializeField] int maxEnemies = 2;
    [SerializeField] bool spawnEnemies = false;
    [SerializeField] bool isBossRoom = false;
    [SerializeField] List<GameObject> enemies;

    LevelManager levelManager = null;
    public bool allEnemiesDead = false;
    private bool isBossDead;
    public int numberOfEnemies = 0;

    /// TEEEST
    private void Update() {
        if (Input.GetKeyDown("g")) {
            levelManager.OpenAllDoors();
        }
    }

    void Start() {
        levelManager = gameObject.GetComponentInParent<LevelManager>();
        if (spawnEnemies) {
            allEnemiesDead = false;
            SpawnEnemies();
        }
        else if (enemies != null && enemies.Count > 0) {
            numberOfEnemies = enemies.Count;
        }
        else {
            allEnemiesDead = true;
            levelManager.OpenAllDoors();
        }
    }
    public void AddEnemy(GameObject enemy) {
        enemies.Add(enemy);
        numberOfEnemies++;
        allEnemiesDead = false;
    }
    private void SpawnEnemies() {
        numberOfEnemies = Random.Range(1, maxEnemies + 1);
        CheckIfAllDead();

        if (isBossRoom) {
            GameObject enemy = Instantiate(bossPrefab, new Vector3(7, -3, 0), Quaternion.identity, transform);
            isBossDead = false;
        }
        else {
            for (int i = 0; i < numberOfEnemies; i++) {
                GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                    new Vector3(Random.Range(1, 13), Random.Range(-7, -1), 0),
                    Quaternion.identity, transform);
                enemy.GetComponent<Enemy>().counter = this;
            }
        }
    }

    public void EnemyDead() {
        numberOfEnemies--;
        CheckIfAllDead();
        Debug.Log("Enemy dead, left enemies - " + numberOfEnemies);
    }

    public void BossDead() {
        isBossDead = true;
        numberOfEnemies = 0;
        CheckIfAllDead();
        Debug.Log("Boss dead");
    }

    public void CheckIfAllDead() {
        if (numberOfEnemies <= 0) {
            allEnemiesDead = true;
            levelManager.OpenAllDoors();
            if (isBossRoom) {
                nextLevelTile.SetActive(true);
            }
            else if (isBossDead) {
                nextLevelTile.SetActive(true);
            }
        }
        if (isBossRoom && isBossDead) {
            allEnemiesDead = true;
            nextLevelTile.SetActive(true);
            levelManager.OpenAllDoors();
        }
    }

    public int GetNumberOfEnemies() {
        return numberOfEnemies;
    }

}
