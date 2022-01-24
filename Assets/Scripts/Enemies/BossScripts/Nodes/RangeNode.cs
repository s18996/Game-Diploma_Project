using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node {

    private float range;
    private Transform player;
    private Transform enemy;

    public RangeNode(float range, Transform player, Transform enemy) {
        this.range = range;
        this.player = player;
        this.enemy = enemy;
    }

    public override NodeState Evaluate() {
        float dist = Vector3.Distance(player.position, enemy.position);
        return dist <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
