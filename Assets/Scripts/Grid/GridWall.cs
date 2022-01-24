using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWall : MonoBehaviour {

    GridManager grid;

    private void Awake() {
        grid = GetComponentInParent<GridManager>();
    }

    private void Start() {
        grid.SetStaticField(new Vector2Int((int)transform.position.x, -(int)transform.position.y));
        print("WALL: " + (int)transform.position.x + " : " + -(int)transform.position.y);
    }
}
