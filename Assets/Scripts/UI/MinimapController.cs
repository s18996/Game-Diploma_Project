using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour {

    public GameObject normalRoom;
    public GameObject bossRoom;
    public GameObject upgradeRoom;
    public float notInRoomTransparency;
    public float inRoomTransparency;

    public Transform child;
    public List<GameObject> rooms;
    public Dictionary<Vector2Int, int> roomIndexes = new Dictionary<Vector2Int, int>();

    private int gridSize;
    private Vector2Int currentPos;

    public void ClearRooms() {
        currentPos = new Vector2Int();
        roomIndexes = new Dictionary<Vector2Int, int>();
        rooms = new List<GameObject>();
        foreach (Transform room in child) {
            Destroy(room.gameObject);
        }
    }

    public void CreateRooms(int levelGridSize, List<Room> listOfRooms) {
        ClearRooms();
        gridSize = levelGridSize;
        currentPos = new Vector2Int(levelGridSize / 2, levelGridSize / 2);
        List<Vector2Int> roomPositions = new List<Vector2Int>();
        foreach (Room room in listOfRooms) {
            roomPositions.Add(room.GetPosition());
        }
        int roomIndex = 0;
        int index = 0;
        for (int y = gridSize - 1; y >= 0; y--) {
            for (int x = 0; x < gridSize; x++) {
                if (!roomPositions.Contains(new Vector2Int(x, y))) {
                    GameObject room = Instantiate(normalRoom, child);
                    rooms.Add(room);
                    room.GetComponent<Image>().color = new Color(0,0,0,0);
                }
                else {
                    roomIndexes.Add(new Vector2Int(x, y), index);
                    if (listOfRooms[roomIndex].roomType.Equals(RoomType.Boss)) {
                        rooms.Add(Instantiate(bossRoom, child));
                    }
                    else if (listOfRooms[roomIndex].roomType.Equals(RoomType.Upgrade)) {
                        rooms.Add(Instantiate(upgradeRoom, child));
                    }
                    else {
                        rooms.Add(Instantiate(normalRoom, child));
                    }
                    roomIndex++;
                }
                index++;
            }
        }
        SetMinimapRoomsTransparency();
        UpdatePlayerPosition(currentPos);
    }

    private void SetMinimapRoomsTransparency() {
        foreach (GameObject gameObject in rooms) {
            Image image = gameObject.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, notInRoomTransparency);
        }
    }

    public void UpdatePlayerPosition(Vector2Int position) {
        roomIndexes.TryGetValue(currentPos, out int roomIndex);
        Image image = rooms[roomIndex].GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, notInRoomTransparency);

        roomIndexes.TryGetValue(position, out roomIndex);
        currentPos = position;
        image = rooms[roomIndex].GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, inRoomTransparency);
    }
}
