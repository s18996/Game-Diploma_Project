using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyNode : Node {

    private GameObject[] enemies;
    private BossSpriteChanger spriteChanger;
    private Enemy[] enemyScripts;
    private Vector2[] enemyPositions;
    private EnemiesCounter counter;
    private float spawnCooldown;
    private float timer;

    public SpawnEnemyNode(GameObject[] enemies, EnemiesCounter counter, float spawnCooldown, BossSpriteChanger spriteChanger) {
        this.enemies = enemies;
        this.counter = counter;
        this.spawnCooldown = spawnCooldown;
        this.spriteChanger = spriteChanger;
        timer = spawnCooldown;
        enemyPositions = new Vector2[enemies.Length];
        enemyScripts = new Enemy[enemies.Length];
        for (int i = 0; i < enemies.Length; i++) {
            enemyPositions[i] = enemies[i].transform.position;
            enemyScripts[i] = enemies[i].GetComponent<Enemy>();
            enemies[i].SetActive(false);
        }
    }

    public override NodeState Evaluate() {
        if (timer <= 0) {
            for (int i = 0; i < enemies.Length; i++) {
                Debug.Log("check " + i);
                if (!enemies[i].activeInHierarchy) {
                    Debug.Log("checking " + i);
                    enemies[i].transform.position = enemyPositions[i];
                    enemyScripts[i].SetMaxHealth();
                    counter.AddEnemy(enemies[i]);
                    enemies[i].SetActive(true);
                    timer = spawnCooldown;
                    if (i == 0)
                        spriteChanger.LeftSpawn();
                    else
                        spriteChanger.RightSpawn();
                    return NodeState.SUCCESS;
                }
            }
            return NodeState.FAILURE;
        }
        else {
            timer -= Time.deltaTime;
        }
        return NodeState.RUNNING;
    }
}
