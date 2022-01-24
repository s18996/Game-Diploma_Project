using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node {

    private Enemy enemy;
    private float threshold;

    public HealthNode(Enemy enemy, float threshold) {
        this.enemy = enemy;
        this.threshold = threshold;
    }

    public override NodeState Evaluate() {
        return enemy.GetHealth() <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
