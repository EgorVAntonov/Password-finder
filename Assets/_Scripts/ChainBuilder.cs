using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChainBuilder : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform dot;
    [SerializeField] private GridCreator grid;
    [SerializeField] private LevelValidator validator;
    [Space]
    [SerializeField] private LineRenderer line;
    //[SerializeField] private LineRenderer[] lines;
    [Space]
    [SerializeField] private List<CircleUnit> circlesChain;

    private Vector3 currentInputPosition;
    private CircleUnit touchedCircle;
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
                currentInputPosition = new Vector3(hit.point.x, hit.point.y, 0f);
                SetLastPointOfLineToCurrentTouchPos();
            }

            dot.position = currentInputPosition;
        }
    }

    private void SetLastPointOfLineToCurrentTouchPos()
    {
        if (line.positionCount > 1)
        {
            line.SetPosition(line.positionCount - 1, currentInputPosition);
        }
    }

    public void HandleUnitTouch(CircleUnit unit)
    {
        touchedCircle = unit;

        if (line.positionCount == 0)
        {
            circlesChain.Add(touchedCircle);
            StartDraw(unit);
            return;
        }
        CheckCircleSkipping();
        circlesChain.Add(touchedCircle);
        AppendLine(touchedCircle.unitPosition);
        DefineNewSegment();
    }

    private void CheckCircleSkipping()
    {
        CircleUnit midUnit = grid.GetCircleBetween(circlesChain.ToArray()[circlesChain.Count - 1], touchedCircle);
        while (midUnit != null)
        {
            circlesChain.Add(midUnit);
            AppendLine(midUnit.unitPosition);
            midUnit.isInChain = true;
            DefineNewSegment();

            midUnit = grid.GetCircleBetween(circlesChain.ToArray()[circlesChain.Count - 1], touchedCircle);
        }
    }

    private void DefineNewSegment()
    {
        CircleUnit[] circlesChainArray = circlesChain.ToArray();
        Vector2 segmentOffset = circlesChainArray[circlesChainArray.Length-1].unitIndexes - circlesChainArray[circlesChainArray.Length - 2].unitIndexes;
        validator.AddToCurrentKit(segmentOffset);
    }

    private void AppendLine(Vector3 nextPosition)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 2, nextPosition);
    }

    private void StartDraw(CircleUnit unit)
    {
        circlesChain.Add(unit);
        isBuilding = true;
        line.positionCount = 2;
        line.SetPosition(0, unit.unitPosition);
    }

    public void EraseLine()
    {
        if (isBuilding == false) return;

        isBuilding = false;
        line.positionCount = 0;
        foreach (var circle in circlesChain)
        {
            circle.isInChain = false;
        }
        circlesChain.Clear();
        validator.ClearCurrentKit();
    }
}
