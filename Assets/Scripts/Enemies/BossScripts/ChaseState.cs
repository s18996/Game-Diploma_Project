using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IBossState {

    public IBossState UseState(BossStateMachine boss) {

        ChasePlayer(boss);

        if (boss.canChangeState) {
            boss.stateTimer = boss.stateChangeTime;
            if (boss.CanSeePlayer() && !IsInChasingRange(boss)) {
                boss.StateChange();
                return boss.shootState;
            }
            else if (boss.CanSeePlayer() && IsInRunningRange(boss)) {
                boss.StateChange();
                return boss.runState;
            }
        }

        return boss.chaseState;
    }

    private bool IsInChasingRange(BossStateMachine boss) {
        float distance = Vector2.Distance(boss.transform.position, boss.player.transform.position);
        return distance > boss.chasingRange;
    }

    private bool IsInRunningRange(BossStateMachine boss) {
        float distance = Vector2.Distance(boss.transform.position, boss.player.transform.position);
        return distance < boss.runningRange;
    }

    private void ChasePlayer(BossStateMachine boss) {
        if (boss.CanSeePlayer() && IsInChasingRange(boss)) {
            boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, boss.player.transform.position, boss.chaseSpeed * Time.fixedDeltaTime));
        }
        else if (!boss.CanSeePlayer()) {
            boss.chaseTimer -= Time.deltaTime;
            if (boss.chaseTimer <= 0) {
                boss.path = boss.aStar.FindPath(boss.grid.GetField(boss.transform.position), boss.grid.GetField(boss.player.transform.position));
                boss.chaseTimer = boss.aStarPathCooldown;
            }
            if (boss.grid.GetField(boss.transform.position) == boss.grid.GetField(boss.player.transform.position)) {
                boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, boss.player.transform.position, boss.chaseSpeed * Time.fixedDeltaTime));
            }
            else if (boss.path != null && boss.path.Count > 1) {
                boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, new Vector3(boss.path[1].x, boss.path[1].y, 0), boss.chaseSpeed * Time.fixedDeltaTime));
                if(boss.path.Count > 2 && Vector2.Distance(boss.transform.position, new Vector2(boss.path[1].x, boss.path[1].y)) < 0.3f) {
                    boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, new Vector3(boss.path[2].x, boss.path[2].y, 0), boss.chaseSpeed * Time.fixedDeltaTime));
                }
            }
        }
        boss.spriteChanger.DefaultSprite();
    }
}
