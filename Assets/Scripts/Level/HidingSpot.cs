using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour {

    public int col;
    public int row;

    private void Start() {
        col = (int)transform.position.x;
        row = (int)Mathf.Abs(transform.position.y);
    }

}
