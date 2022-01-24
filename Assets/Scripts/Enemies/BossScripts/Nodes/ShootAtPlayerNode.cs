using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerNode : Node {

    private Transform player;
    private Transform enemy;
    private BossSpriteChanger spriteChanger;
    private string tag;
    private float shotCooldown;
    private float timer;

    public ShootAtPlayerNode(Transform player, Transform enemy, string tag, float shotCooldown, BossSpriteChanger spriteChanger) {
        this.player = player;
        this.enemy = enemy;
        this.tag = tag;
        this.shotCooldown = shotCooldown;
        this.spriteChanger = spriteChanger;
        timer = shotCooldown;
    }

    public override NodeState Evaluate() {
        // SHOOT AT PLAYER
        if(timer <= 0) {
            Debug.Log("SHOOTING AT PLAYER");
            GameObject projectile = ObjectPooling.Instance.SpawnFromPool(tag, enemy.position,
                Quaternion.Euler(new Vector3(0, 0, AngleBetweenTwoPoints(enemy.position, player.transform.position) - 90f)));
            projectile.GetComponent<Projectile>().StartMove(enemy.transform, player.transform);
            timer = shotCooldown;
            spriteChanger.DefaultSprite();
            return NodeState.SUCCESS;
        }
        else {
            timer -= Time.deltaTime;
        }
        return NodeState.FAILURE;
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}
