using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelValidator : MonoBehaviour
{
    [SerializeField] private SegmentPresentorsController presentors;
    [SerializeField] private LevelCreator creator;

    [SerializeField] private SegmentData[] allSegments;

    private List<Segment> targetKit;
    private List<Segment> currentKit;
    private bool levelCompleted;
    private bool canMoveToNextlevel;

    private void Start()
    {
        levelCompleted = false;
        canMoveToNextlevel = false;
        targetKit = new List<Segment>();
        currentKit = new List<Segment>();
    }

    private void Update()
    {
        if (canMoveToNextlevel && Input.touchCount == 0)
        {
            canMoveToNextlevel = false;
            creator.CreateLevel();
        }
    }

    public void ClearTargetKit()
    {
        targetKit.Clear();
    }

    public void AddToTargetKit(Vector2 offset)
    {
        AddToKit(targetKit, offset);
    }

    public void SetupPresentors()
    {
        presentors.CreatePresentors(targetKit);
    }

    public void AddToCurrentKit(Vector2 offset)
    {
        AddToKit(currentKit, offset);
        FindMatches();
    }

    private void AddToKit(List<Segment> kit, Vector2 offset)
    {
        SegmentData data = GetSegmentData(offset);

        Segment[] kitArray = kit.ToArray();
        bool alreadyInKit = false;
        for (int i = 0; i < kitArray.Length; i++)
        {
            if (kitArray[i].data == data)
            {
                kitArray[i].count++;
                alreadyInKit = true;
            }
        }
        if (alreadyInKit) return;
        Segment newSegment = new Segment(data, 1);
        kit.Add(newSegment);
    }

    private SegmentData GetSegmentData(Vector2 offset)
    {
        for (int i = 0; i < allSegments.Length; i++)
        {
            for (int j = 0; j < allSegments[i].offsets.Length; j++)
            {
                if (allSegments[i].offsets[j] == offset)
                {
                    return allSegments[i];
                }
            }
        }
        Debug.LogError("return null segmentData; offset is " + offset.ToString());
        return null;
    }

    private void FindMatches()
    {
        levelCompleted = true;
        foreach (var targetSegment in targetKit)
        {
            SegmentType type = targetSegment.data.type;
            Segment currentSegment = currentKit.Find(seg => seg.data.type == type); 

            if (currentSegment == null)
            {
                presentors.ShowCountMatch(type, targetSegment.count, 0);
                levelCompleted = false;
            }
            else
            {
                presentors.ShowCountMatch(type, targetSegment.count, currentSegment.count);
                if (targetSegment.count != currentSegment.count)
                {
                    levelCompleted = false;
                }
            }
        }
        if (levelCompleted)
        {
            canMoveToNextlevel = true;
        }
    }

    public void ClearCurrentKit()
    {
        currentKit.Clear();
        FindMatches();
    }
}
