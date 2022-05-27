using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SegmentType
{
    _1x1_horizontal,
    _2x1_horizontal,
    _1x1_vertical,
    _1x2_vertical,
    _1x1_diagonal_NE,
    _1x1_diagonal_SE,
    _1x2_diagonal_NE,
    _1x2_diagonal_SE,
    _2x1_diagonal_NE,
    _2x1_diagonal_SE,
    _2x2_diagonal_NE,
    _2x2_diagonal_SE,
}
[CreateAssetMenu(fileName = "Segment", menuName = "New Segment")]
public class SegmentData : ScriptableObject
{
    public SegmentType type;
    public Vector2[] offsets;
    public Sprite icon;
}
