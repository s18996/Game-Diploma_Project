using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding {

    private static readonly float DIAGONALLY = 1.41f;
    private static readonly float STRAIGHT = 1f;

    private List<Field> queue;
    private List<Field> visited;
    private GridManager gm = GameObject.Find("Grid").GetComponent<GridManager>();

    public List<Field> FindPath(Field startField, Field endField) {
        queue = new List<Field>();
        visited = new List<Field>();

        float hScore = LowestDistance(startField, endField);
        for (int col = 0; col < gm.cols; col++) {
            for (int row = 0; row < gm.rows; row++) {
                Field field = gm.GetField(col, row);
                field.g = Mathf.Infinity;
                field.h = hScore;
                field.f = Mathf.Infinity;
                field.lastField = null;
            }
        }
        startField.g = 0;
        startField.h = hScore;
        startField.f = startField.h;
        queue.Add(startField);

        while (queue.Count > 0) {
            Field current = GetLowestFScoreField(queue);
            if (current == endField)
                return ReconstructPath(current);
            queue.Remove(current);
            visited.Add(current);
            foreach (Field neighbour in GetNeighbours(current)) {
                if (visited.Contains(neighbour))
                    continue;
                float tentative_gScore = current.g + LowestDistance(current, neighbour);
                if (tentative_gScore < neighbour.g) {
                    neighbour.lastField = current;
                    neighbour.g = tentative_gScore;
                    neighbour.h = LowestDistance(neighbour, endField);
                    neighbour.f = neighbour.g + neighbour.h;
                    if (!queue.Contains(neighbour))
                        queue.Add(neighbour);
                }
            }
        }
        return null;
    }

    private List<Field> ReconstructPath(Field current) {
        List<Field> path = new List<Field>() { current };
        while (current.lastField != null) {
            current = current.lastField;
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private float LowestDistance(Field first, Field second) {
        float xDist = Mathf.Abs(first.col - second.col);
        float yDist = Mathf.Abs(first.row - second.row);
        float straightMoves = Mathf.Abs(xDist-yDist);
        float diagonalMoves = xDist;
        if (yDist < xDist)
            diagonalMoves = yDist;
        return DIAGONALLY * diagonalMoves + STRAIGHT * straightMoves;
    }

    private List<Field> GetNeighbours(Field current) {
        List<Field> neighbours = new List<Field>();
        if (current.row - 1 >= 0)
            if (!gm.GetField(current.col, current.row - 1).isWall)
                neighbours.Add(gm.GetField(current.col, current.row - 1));
        if (current.row + 1 < gm.rows)
            if (!gm.GetField(current.col, current.row + 1).isWall)
                neighbours.Add(gm.GetField(current.col, current.row + 1));
        if (current.col - 1 >= 0)
            if (!gm.GetField(current.col - 1, current.row).isWall)
                neighbours.Add(gm.GetField(current.col - 1, current.row));
        if (current.col + 1 < gm.cols)
            if (!gm.GetField(current.col + 1, current.row).isWall)
                neighbours.Add(gm.GetField(current.col + 1, current.row));

        if (current.col - 1 >= 0 && current.row - 1 >= 0
                && !gm.GetField(current.col - 1, current.row).isWall
                && !gm.GetField(current.col, current.row - 1).isWall
                && !gm.GetField(current.col - 1, current.row - 1).isWall) {
            neighbours.Add(gm.GetField(current.col - 1, current.row - 1));
        }
        if (current.col + 1 < gm.cols && current.row - 1 >= 0 
                && !gm.GetField(current.col + 1, current.row).isWall
                && !gm.GetField(current.col, current.row - 1).isWall
                && !gm.GetField(current.col + 1, current.row - 1).isWall) {
            neighbours.Add(gm.GetField(current.col + 1, current.row - 1));
        }
        if (current.col - 1 >= 0 && current.row + 1 < gm.rows
                && !gm.GetField(current.col - 1, current.row).isWall
                && !gm.GetField(current.col, current.row + 1).isWall
                && !gm.GetField(current.col - 1, current.row + 1).isWall) {
            neighbours.Add(gm.GetField(current.col - 1, current.row + 1));
        }
        if (current.col + 1 < gm.cols && current.row + 1 < gm.rows
                && !gm.GetField(current.col + 1, current.row).isWall
                && !gm.GetField(current.col, current.row + 1).isWall
                && !gm.GetField(current.col + 1, current.row + 1).isWall) {
            neighbours.Add(gm.GetField(current.col + 1, current.row + 1));
        }
        return neighbours;
    }

    private Field GetLowestFScoreField(List<Field> fields) {
        Field lowestF = fields[0];
        foreach (Field field in fields) {
            if (field.f < lowestF.f)
                lowestF = field;
        }
        return lowestF;
    }
}
