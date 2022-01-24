using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSpawnNode : Node {

    private EnemiesCounter counter;

    public CanSpawnNode(EnemiesCounter counter) {
        this.counter = counter;
    }

    public override NodeState Evaluate() {
        return counter.GetNumberOfEnemies() > 2 ? NodeState.FAILURE : NodeState.SUCCESS;
    }
}
