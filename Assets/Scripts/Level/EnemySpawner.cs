using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //private void SpawnEnemies() {
    //    EnemiesCounter counter = transform.parent.GetComponent<EnemiesCounter>();
    //    int numberOfEnemies = counter.GetNumberOfEnemies();
    //    int maxEnemies = counter.GetMaxEnemies();
    //    int numberOfEnemies = Random.Range(0, maxEnemies + 1);
    //    if (numberOfEnemies == 0) {
    //        allEnemiesDead = true;
    //        levelManager.OpenAllDoors();
    //    }

    //    for (int i = 0; i < numberOfEnemies; i++) {
    //        GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
    //            new Vector3(Random.Range(1, 13), Random.Range(-7, -1), 0),
    //            Quaternion.identity, transform);
    //        enemy.GetComponent<Enemy>().counter = this;
    //    }
    //}
}
