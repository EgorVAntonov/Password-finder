using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GameObject unitCirclePrefab;

    private CircleUnit[,] grid;

    [SerializeField] private int width;
    [SerializeField] private int height;

    public Vector2 indexesDelta;
    public Vector2 absDelta;
    public Vector2 clampedDelta;
    public Vector2 midUnitIndexes;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
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
            }
        }

        Vector3 GetPositionForCell(int x, int y)
        {
            return new Vector3(x - (width / 2f) + 0.5f, y - (height / 2f) + 0.5f, 0f);
        }
    }

    public CircleUnit GetCircleBetween(CircleUnit first, CircleUnit second)
    {
       indexesDelta = second.unitIndexes - first.unitIndexes;
       absDelta = new Vector2(Mathf.Abs(indexesDelta.x), Mathf.Abs(indexesDelta.y));
        if (absDelta.x != absDelta.y) 
        {
            if (absDelta.x > 0f && absDelta.y > 0f)
            {
                Debug.Log("return null mid circle");
                return null;
            }
        }
        /*
        if (absDelta.x == absDelta.y ||
            absDelta.x == 0f && absDelta.y > 0f ||
            absDelta.x > 0f && absDelta.y == 0f)
        {

        }
        */
        clampedDelta = new Vector2(Mathf.Clamp(indexesDelta.x, -1f, 1f), Mathf.Clamp(indexesDelta.y, -1f, 1f));
        midUnitIndexes = first.unitIndexes + clampedDelta;
        CircleUnit midUnit = grid[Mathf.RoundToInt(midUnitIndexes.x), Mathf.RoundToInt(midUnitIndexes.y)];
        if (midUnit != null && midUnit.isOccupied == false)
        {
            return midUnit;
        }
        else
        {
            Debug.Log("can not find mid circle or it is occupied");
        }
        return null;
    }
}