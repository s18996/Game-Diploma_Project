using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : IBossState {

    public IBossState UseState(BossStateMachine boss) {
        boss.stateTimer = boss.stateChangeTime;

        Hide(boss);

        return boss.hideState;
    }

    private void Hide(BossStateMachine boss) {
        boss.pathTimer -= Time.deltaTime;
        Field spot = GetFurthestField(boss);
        if (boss.path != null && boss.pathTimer > 0) {
            MoveToSpot(boss, spot);
        }
        else {
            boss.path = boss.aStar.FindPath(boss.grid.GetField(boss.transform.position), spot);
            boss.pathTimer = boss.aStarPathCooldown;
        }
    }

    private void MoveToSpot(BossStateMachine boss, Field spot) {
        if (boss.grid.GetField(boss.transform.position) == spot) {
            boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, new Vector2(spot.col, -spot.row), boss.hidingSpeed * Time.fixedDeltaTime));
        }
        else if (boss.path != null && boss.path.Count > 1) {
            boss.rb.MovePosition(Vector3.MoveTowards(boss.transform.position, new Vector3(boss.path[1].col, -boss.path[1].row, 0), boss.hidingSpeed * Time.fixedDeltaTime));
        }
    }

    private Field GetFurthestField(BossStateMachine boss) {
        HidingSpot furthest = boss.spots[0];
        float furthestDist = Vector2.Distance(boss.player.transform.position, boss.spots[0].transform.position);
        for (int i = 1; i < boss.spots.Count; i++) {
            float dist = Vector2.Distance(boss.player.transform.position, boss.spots[i].transform.position);
            if(dist > furthestDist) {
                furthestDist = dist;
                furthest = boss.spots[i];
            }
        }
        return boss.grid.GetField(furthest.col, furthest.row);
    }


}
