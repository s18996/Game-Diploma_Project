using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField] private float followingSpeed = 0;
    [SerializeField] private bool usingAStar = true;
    [SerializeField] private float startTime = 0.5f;
    [SerializeField] private float newPathTimer = 0.05f;

    private GameObject targetPlayer = null;
    private AStarPathfinding aStar = null;
    private GridManager grid;
    private List<Field> path;

    private float timer;

    void Awake() {
        targetPlayer = GameManager.player;
    }

    void Start() {
        grid = GameObject.Find("Grid").GetComponent<GridManager>();
        aStar = new AStarPathfinding();
        path = NewPath();
        timer = newPathTimer;
    }

    void Update() {
        if (startTime > 0) {
            startTime -= Time.deltaTime;
            return;
        }
        if (usingAStar) {
            if (startTime > 0)
                startTime -= Time.deltaTime;
            else {
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    path = NewPath();
                    timer = newPathTimer;
                }
                MoveToNextField();
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, followingSpeed * Time.deltaTime);
        }
    }

    void MoveToNextField() {
        if(grid.GetField(transform.position) == grid.GetField(targetPlayer.transform.position)) {
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, followingSpeed * Time.deltaTime);
        }
        else if (path != null && path.Count > 1) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[1].x, path[1].y, 0), followingSpeed * Time.deltaTime); // moves towards the next square in A*
            if (path.Count > 2 && Vector2.Distance(transform.position, new Vector2(path[1].x, path[1].y)) < 0.3f) {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[2].x, path[2].y, 0), followingSpeed * Time.deltaTime);
            }
        }
    }

    List<Field> NewPath() {
        return aStar.FindPath(grid.GetField(transform.position), grid.GetField(targetPlayer.transform.position));
    }


}
