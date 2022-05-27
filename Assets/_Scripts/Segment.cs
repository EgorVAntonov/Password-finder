using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    public SegmentData data;
    public int count;

    public Segment(SegmentData _data, int _count)
    {
        data = _data;
        count = _count;
    }
}
