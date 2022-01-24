using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : IBossState {

    public IBossState UseState(BossStateMachine boss) {

        if (boss.CanSeePlayer()) {
            ShootAtPlayer(boss);
        }

        if (boss.canChangeState) {
            boss.stateTimer = boss.stateChangeTime;
            float distance = Vector2.Distance(boss.transform.position, boss.player.transform.position);
            if (IsInRunningRange(boss, distance)) {
                boss.StateChange();
                return boss.runState;
            }
            if (IsInChasingRange(boss, distance) || !boss.CanSeePlayer()) {
                boss.StateChange();
                return boss.chaseState;
            }
        }

        return boss.shootState;
    }

    private bool IsInRunningRange(BossStateMachine boss, float distance) {
        return distance < boss.runningRange;
    }

    private bool IsInChasingRange(BossStateMachine boss, float distance) {
        return distance > boss.chasingRange;
    }

    private void ShootAtPlayer(BossStateMachine boss) {
        float dur = 1f / boss.shotsPerSecond;
        boss.shotTime += Time.deltaTime;
        while (boss.shotTime >= dur) {
            boss.shotTime -= dur;
            boss.ShootAtPlayer();
        }
    }
}
