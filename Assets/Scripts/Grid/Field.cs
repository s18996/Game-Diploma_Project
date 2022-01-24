using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field {

    public ObjectType objectType;
    public int col;
    public int row;
    public float g;
    public float h;
    public float f;
    public int x;
    public int y;

    public bool isWall;
    public Field lastField;

    public Field(int col, int row, ObjectType objectType = ObjectType.Nothing) {
        SetField(col, row, objectType);
    }

    public void SetField(int col, int row, ObjectType objectType) {
        this.col = col;
        x = col;
        y = -row;
        this.row = row;
        this.objectType = objectType;
        isWall = false;
        if (objectType == ObjectType.Wall || objectType == ObjectType.Destructible)
            isWall = true;
    }

    public override string ToString() {
        return col + ", " + row;
    }

    public void RestartFieldType() {
        objectType = ObjectType.Nothing;
        isWall = false;
    }
}

public enum ObjectType {
    Nothing,
    Wall,
    Destructible
}