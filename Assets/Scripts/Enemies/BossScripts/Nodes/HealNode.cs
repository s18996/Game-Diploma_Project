using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealNode : Node {

    private Enemy enemy;
    private BossSpriteChanger spriteChanger;
    private float healValue;
    private float healCooldown;
    private float timer;

    public HealNode(Enemy enemy, float healValue, float healCooldown, BossSpriteChanger spriteChanger) {
        this.enemy = enemy;
        this.healValue = healValue;
        this.healCooldown = healCooldown;
        this.spriteChanger = spriteChanger;
        timer = healCooldown;
    }

    public override NodeState Evaluate() {
        if (timer <= 0) {
            Debug.Log("HEALING");
            enemy.Heal(healValue);
            timer = healCooldown;
            spriteChanger.DefaultSprite();
            return NodeState.SUCCESS;
        }
        else {
            timer -= Time.deltaTime;
        }
        return NodeState.RUNNING;
    }
}
