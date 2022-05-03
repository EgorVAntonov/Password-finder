using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChainBuilder : MonoBehaviour
{
    [SerializeField] private Transform testObject;
    [SerializeField] private GridCreator grid;
    [Space]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Camera _camera;
    [Space]
    [SerializeField] private List<CircleUnit> circlesChain;

    private Vector3 currentInputPosition;
    private CircleUnit nextCircle;

    private bool isBuilding;

    void Start()
    {
        isBuilding = false;

        _camera = Camera.main;

        line.positionCount = 0;
    }
    
    void Update()
    {
        if (isBuilding == false) return;

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount == 0) 
        {
            EraseLine();
            return;
        }

        if (Input.touchCount > 0)
        {
            Vector2 fingerPosition = Input.GetTouch(0).position;
            Ray ray = _camera.ScreenPointToRay(fingerPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentInputPosition = hit.point;
                SetLastPointOfLineToCurrentTouchPos();
            }

            testObject.position = CutVectorOnZ(currentInputPosition);
        }
    }

    private void SetLastPointOfLineToCurrentTouchPos()
    {
        if (line.positionCount > 1)
        {
            line.SetPosition(line.positionCount - 1, CutVectorOnZ(currentInputPosition));
        }
    }

    public void HandleUnitTouch(CircleUnit unit)
    {
        nextCircle = unit;

        if (line.positionCount == 0)
        {
            circlesChain.Add(nextCircle);
            StartBuild(unit);
            return;
        }
        CircleUnit midUnit = grid.GetCircleBetween(circlesChain.ToArray()[circlesChain.Count - 1], nextCircle);
        while (midUnit != null)
        {
            circlesChain.Add(midUnit);
            midUnit.isOccupied = true;
            AppendLine(midUnit.transform.position);
            midUnit = grid.GetCircleBetween(circlesChain.ToArray()[circlesChain.Count - 1], nextCircle);
        }
        circlesChain.Add(nextCircle);
        AppendLine(nextCircle.transform.position);
    }

    private void AppendLine(Vector3 nextPosition)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 2, CutVectorOnZ(nextPosition));
    }

    private void StartBuild(CircleUnit unit)
    {
        circlesChain.Add(unit);
        isBuilding = true;
        line.positionCount = 2;
        line.SetPosition(0, CutVectorOnZ(unit.transform.position));
    }

    public void EraseLine()
    {
        if (isBuilding == false) return;

        isBuilding = false;
        line.positionCount = 0;
        foreach (var circle in circlesChain)
        {
            circle.isOccupied = false;
        }
        circlesChain.Clear();
    }

    private Vector3 CutVectorOnZ(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0f);
    }
}
