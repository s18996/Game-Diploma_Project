using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    [SerializeField] GameObject[] floorSprites = null;
    [SerializeField] GameObject[] materialObjects = null;
    [SerializeField] GameObject border = null;
    [SerializeField] int objectsCount = 10;

    public List<GameObject> materials;
    public int rows = 9;
    public int cols = 15;
    List<List<Field>> fields;
    List<Vector2> objectPositions;
    public List<Vector2> obstacles;

    void Awake() {
        //border = Instantiate(border, transform);
        obstacles = new List<Vector2>();
        fields = new List<List<Field>>();
        if(materials.Count < 1) {
            materials = new List<GameObject>();
        }
        objectPositions = RandomObjectPositions(objectsCount);
        CreateGrid();
    }

    private List<Vector2> RandomObjectPositions(int count) {
        List<Vector2> vectors = new List<Vector2>();
        for (int i = 0; i < count; i++) {
            Vector2 vector2 = new Vector2((int)(Random.value*cols), (int)(Random.value*rows));
            while (vectors.Contains(vector2))
                vector2 = new Vector2((int)(Random.value*cols), (int)(Random.value*rows));
            vectors.Add(vector2);
        }
        return vectors;
    }

    private void CreateGrid() {
        for (int col = 0; col < cols; col++) {
            fields.Add(new List<Field>());
            for (int row = 0; row < rows; row++) {
                GameObject tile;
                GameObject material = null;
                int random = Random.Range(0, floorSprites.Length);
                if (objectPositions.Contains(new Vector2(col, row))) {
                    fields[col].Add(new Field(col, row, ObjectType.Destructible));
                    tile = Instantiate(floorSprites[random], transform);
                    material = Instantiate(materialObjects[ChooseMaterialIndex()], transform);
                }
                else {
                    fields[col].Add(new Field(col, row));
                    tile = Instantiate(floorSprites[random], transform);
                }
                float posX = col;
                float posY = -row;

                if (material != null) {
                    material.transform.position = new Vector2(posX, posY);
                    materials.Add(material);
                }
                tile.transform.position = new Vector2(posX, posY);
                
            }
        }
        Camera.main.transform.position = new Vector3(cols/2f - 0.5f, -rows/2f + 0.5f, -10);
        Camera.main.orthographicSize = cols/2.8f;
    }

    private int ChooseMaterialIndex() {
        if (PrefsManager.IsCrystalOreUpgraded()) {
            int random = Random.Range(0, 100);
            return random < 70 ? 0 : 1;
        }
        else {
            return 0;
        }
    }

    public Field GetField(int col, int row) {
        return fields[col][row];
    }

    public void SetStaticField(Vector2Int vector2) {
        fields[vector2.x][vector2.y].isWall = true;
    }
    public void SetDestructible(Vector2Int vector2) {
        fields[vector2.x][vector2.y].isWall = true;
        fields[vector2.x][vector2.y].objectType = ObjectType.Destructible;
        print(vector2.x + " " + vector2.y);
    }

    public Field GetField(Vector2 point) {
        int col = (int)(point.x + 0.5f);
        int row = (int)(point.y - 0.5f);
        if (col >= 0 && row <= 0 && col < cols && row > -rows) {
            return fields[col][-row];
        }
        else {
            return null;
        }
    }

    public void SetBorder(Vector2 point) {
        int col = (int)(point.x + 0.5f);
        int row = (int)(point.y - 0.5f);
        if (col >= 0 && row <= 0 && col < cols && row > -rows)
            border.transform.position = new Vector2(col, row);
    }

    public GameObject GetMaterialObject(Vector2 point) {
        int col = (int)(point.x + 0.5f);
        int row = (int)(point.y - 0.5f);
        print("clicked on: " + col + " : " + row);
        foreach (GameObject materialObject in materials) {
            if (materialObject.transform.position.x == col && materialObject.transform.position.y == row)
                return materialObject;
        }
        return null;
    }
    public List<Field> GetNeighbourFields(Field pos) {
        List<Field> neighbours = new List<Field>();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (pos.col + i >= 0 && pos.col + i < cols && pos.row + j >= 0 && pos.row + j < rows) {
                    Field field = GetField(pos.col + i, pos.row + j);
                    if (field != null)
                        neighbours.Add(field);
                }
            }
        }
        return neighbours;
    }


}
