using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChainBuilder : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera _camera;
    [SerializeField] private DoubleClickChecker doubleClick;
    [SerializeField] private GridCreator grid;
    [SerializeField] private LevelValidator validator;
    [Space]
    [SerializeField] private SegmentDrawer[] drawers;
    private SegmentDrawer activeDrawer;
    [Space]

    private List<SegmentDrawer> localChain;
    private Vector3 currentInputPosition;
    private bool isTracking;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        localChain = new List<SegmentDrawer>();
        _camera = Camera.main;
        activeDrawer = drawers[0];
        isTracking = false;
        ClearAllConnections();
    }
    
    private void Update()
    {
        if (isTracking == false) return;

        HandleTracking();
    }

    private void HandleTracking()
    {
        if (Input.touchCount == 0)
        {
            StopTracking();
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
        }
    }

    public void StopTracking()
    {
        isTracking = false;
        CircleUnit.lastTouchedCircle = null;
        if (activeDrawer == null) return;

        activeDrawer.from = null;
        activeDrawer.to = null;
        activeDrawer.line.positionCount = 0;
    }

    private void SetLastPointOfLineToCurrentTouchPos()
    {
        if (activeDrawer == null)
        {
            Debug.Log("active drawer is null");
            return;
        }
        LineRenderer line = activeDrawer.line;
        if (line.positionCount > 1)
        {
            line.SetPosition(line.positionCount - 1, currentInputPosition);
        }
    }

    public void HandleUnitTouch(CircleUnit unit)
    {
        if (CircleUnit.lastTouchedCircle == null)
        {
            TryBeginNewSegment(unit);
            return;
        }

        if (CircleUnit.lastTouchedCircle != unit)
        {
            if (CanHaveConnection(unit) == false) return;
            if (AlreadyHaveSameSegment(unit, CircleUnit.lastTouchedCircle)) return;
            if (IsBeginingOfChain(unit)) return;

            FinishSegmentAndTryToBeginNew(unit);
        }
    }

    private void TryBeginNewSegment(CircleUnit from)
    {
        if (CanHaveConnection(from) == false) return;
        FindAvailableSegmentDrawer();
        if (activeDrawer == null) return;

        Debug.Log("begin new segment with " + from.gameObject.name);
        from.SetLastTouchedCircle();
        activeDrawer.from = from;
        activeDrawer.line.positionCount = 2;
        activeDrawer.line.SetPosition(0, from.unitPosition);
        StartTracking();
    }

    private bool CanHaveConnection(CircleUnit unit)
    {
        int count = 0;
        foreach (var segment in drawers)
        {
            if (segment.HasUnit(unit))
            {
                count++;
            }
        }
        return count < 2;
    }
    private void FindAvailableSegmentDrawer()
    {
        for (int i = 0; i < drawers.Length; i++)
        {
            if (drawers[i].IsDrawn() == false)
            {
                activeDrawer = drawers[i];
                return;
            }
        }
        activeDrawer = null;
    }

    public void StartTracking()
    {
        isTracking = true;
    }

    private bool AlreadyHaveSameSegment(CircleUnit from, CircleUnit to)
    {
        for (int i = 0; i < drawers.Length; i++)
        {
            if (drawers[i].HasUnit(from) && drawers[i].HasUnit(to))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBeginingOfChain(CircleUnit unit)
    {
        GetListOfSegments();
        SegmentDrawer nextDrawer = GetNextDrawerInLocalChain(unit);
        CircleUnit nextUnit = unit;
        while (nextDrawer != null)
        {
            nextUnit = nextDrawer.GetTailOfSegment(nextUnit);
            Debug.Log("next unit is " + nextUnit.gameObject.name);
            if (nextUnit == CircleUnit.lastTouchedCircle)
            {
                return true;
            }
            nextDrawer = GetNextDrawerInLocalChain(nextUnit);
        }

        return false;
    }

    private void GetListOfSegments()
    {
        localChain.Clear();
        for (int i = 0; i < drawers.Length; i++)
        {
            if (drawers[i].IsDrawn())
            {
                localChain.Add(drawers[i]);
            }
        }
    }

    private SegmentDrawer GetNextDrawerInLocalChain(CircleUnit nextUnitInChain)
    {
        foreach (var drawer in localChain)
        {
            if (drawer.HasUnit(nextUnitInChain))
            {
                Debug.Log("next drawer is " + drawer.gameObject.name);
                localChain.Remove(drawer);
                return drawer;
            }
        }
        Debug.Log("next drawer is null");
        return null;
    }

    private void FinishSegmentAndTryToBeginNew(CircleUnit to)
    {
        activeDrawer.to = to;
        activeDrawer.line.SetPosition(1, to.unitPosition);
        DefineSegments();

        if (CanHaveConnection(to))
        {
            Debug.Log(to.gameObject.name + " can has connection");
            TryBeginNewSegment(to);
        }
        else
        {
            Debug.Log(to.gameObject.name + " can NOT has connection");

            CircleUnit.lastTouchedCircle = null;
            isTracking = false;
        }
    }

    private void DefineSegments()
    {
        validator.ClearCurrentKit();
        for (int i = 0; i < drawers.Length; i++)
        {
            if (drawers[i].IsDrawn())
            {
                Vector2 offset = drawers[i].GetSegmentOffset();
                validator.AddToCurrentKit(offset);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId != 0) return;
        Debug.Log("CLICK");
        if (doubleClick.Check())
        {
            Debug.Log("double click");
            ClearAllConnections();
        }
    }

    public void ClearAllConnections()
    {
        for (int i = 0; i < drawers.Length; i++)
        {
            drawers[i].RemoveSegment();
        }
        DefineSegments();
    }

    public void ClearConnections(CircleUnit unit)
    {
        for (int i = 0; i < drawers.Length; i++)
        {
            if (drawers[i].HasUnit(unit))
            {
                drawers[i].RemoveSegment();
            }
        }
        DefineSegments();
    }
}
