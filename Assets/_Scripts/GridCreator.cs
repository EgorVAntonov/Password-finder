using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GameObject unitCirclePrefab;


    private CircleUnit[,] grid;
    [SerializeField] private int width;
    [SerializeField] private int height;
    private bool gridIsCreated = false;

    public void GenerateGrid()
    {
        if (gridIsCreated) return;

        gridIsCreated = true;

        grid = new CircleUnit[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 cellPosition = GetPositionForCell(i, j);
                GameObject currentCell = Instantiate(unitCirclePrefab, cellPosition, Quaternion.identity, transform) as GameObject;

                currentCell.name = "cell[" + i.ToString() + ", " + j.ToString() + "]";
                grid[i, j] = currentCell.GetComponent<CircleUnit>();
                grid[i, j].unitIndexes = new Vector2(i, j);
                grid[i, j].SetUnitPosition();
            }
        }

        Vector3 GetPositionForCell(int x, int y)
        {
            return new Vector3(x - (width / 2f) + 0.5f, y - (height / 2f) + 0.5f, 0f);
        }
    }

    public CircleUnit GetCircleBetween(CircleUnit first, CircleUnit second)
    {
        Vector2 indexesDelta = second.unitIndexes - first.unitIndexes;
        Vector2 absDelta = new Vector2(Mathf.Abs(indexesDelta.x), Mathf.Abs(indexesDelta.y));
        if (absDelta.x != absDelta.y)
        {
            if (absDelta.x > 0f && absDelta.y > 0f)
            {
                return null;
            }
        }
        Vector2 clampedDelta = new Vector2(Mathf.Clamp(indexesDelta.x, -1f, 1f), Mathf.Clamp(indexesDelta.y, -1f, 1f));
        Vector2 midUnitIndexes = first.unitIndexes + clampedDelta;
        CircleUnit midUnit = grid[Mathf.RoundToInt(midUnitIndexes.x), Mathf.RoundToInt(midUnitIndexes.y)];

        if (midUnit == null) return null;
        if (midUnit.isOccupied == true) return null;
        if (midUnit == second) return null;
        
        return midUnit;
    }

    public CircleUnit GetRandomFreeUnit()
    {
        List<CircleUnit> freeCircles = GetFreeCircles();
        if (freeCircles.Count == 0)
        {
            return null;
        }
        else
        {
            CircleUnit randomUnit = freeCircles.ToArray()[Random.Range(0, freeCircles.Count)];
            return randomUnit;
        }

    }

    private List<CircleUnit> GetFreeCircles()
    {
        List<CircleUnit> freeCircles = new List<CircleUnit>();
        foreach (var circle in grid)
        {
            if (circle.isOccupied == false)
            {
                freeCircles.Add(circle);
            }
        }
        return freeCircles;
    }

    public int GetCirclesCount()
    {
        return height * width;
    }
}