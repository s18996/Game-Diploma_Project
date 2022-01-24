using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    // UpDoor-0, RightDoor-1, DownDoor-2, LeftDoor-3
    public bool[] activeDoors = new bool[4];
    public GameObject roomInstance = null;
    Vector2Int position;
    public RoomType roomType;

    public Room(Vector2Int position, RoomType roomType = RoomType.Default) {
        this.roomType = roomType;
        this.position = position;
    }

    public void SetActiveDoors(List<DoorDir> dirs) {
        foreach (DoorDir dir in dirs) {
            activeDoors[(int)dir] = true;
        }
    }

    public Vector2Int GetPosition() {
        return position;
    }

}

public enum DoorDir {
    UpDoor = 0,
    RightDoor = 1,
    DownDoor = 2,
    LeftDoor = 3
}

public enum RoomType {
    Default,
    Boss,
    Upgrade
}

