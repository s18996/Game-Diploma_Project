using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IBossState {

    public IBossState UseState(BossStateMachine boss) {

        RunAway(boss);

        if (boss.canChangeState) {
            boss.stateTimer = boss.stateChangeTime;
            if (boss.enemy.GetHealth() / boss.enemy.GetMaxHealth() < boss.healthThreshold) {
                boss.StateChange();
                return boss.hideState;
            }
            else if (IsFarEnough(boss)) {
                boss.StateChange();
                return boss.shootState;
            }
        }
        return boss.runState;
    }

    private bool IsFarEnough(BossStateMachine boss) {
        float distance = Vector2.Distance(boss.transform.position, boss.player.transform.position);
        return distance > boss.runningRange;
    }

    private void RunAway(BossStateMachine boss) {
        Vector3 targetPosition = boss.transform.position - boss.player.transform.position;
        boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, boss.transform.position + targetPosition, boss.runSpeed * Time.fixedDeltaTime));
        boss.spriteChanger.DefaultSprite();
    }

}
