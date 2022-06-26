using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SegmentDrawer : MonoBehaviour
{
    public CircleUnit from;
    public CircleUnit to;
    public LineRenderer line;

    void Start()
    {
        from = null;
        to = null;
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    public void RemoveSegment()
    {
        from = null;
        to = null;
        line.positionCount = 0;
    }

    public bool IsDrawn()
    {
        return from != null && to != null;
    }

    public bool HasUnit(CircleUnit unit)
    {
        return from == unit || to == unit;
    }

    public Vector2 GetSegmentOffset()
    {
        return from.unitIndexes - to.unitIndexes;
    }

    public CircleUnit GetTailOfSegment(CircleUnit head)
    {
        if (from == head)
        {
            return to;
        }
        else
        {
            return from;
        }
    }
}
