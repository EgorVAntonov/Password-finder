using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentPresentor : MonoBehaviour
{
    public SegmentType segmentType;
    [SerializeField] private Image icon;
    [SerializeField] private Image[] indicators;

    public void Setup(Segment segment)
    {
        if (segment.data == null)
        {
            Debug.LogError("data is null");
            return;
        }
        segmentType = segment.data.type;
        icon.sprite = segment.data.icon;
        int count = segment.count;
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].color = Color.white;
            indicators[i].gameObject.SetActive(i < count);
        }
    }

    public void UpdateCountIndicators(int targetCount, int currentCount)
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            if (i >= targetCount && i >= currentCount)
            {
                indicators[i].gameObject.SetActive(false);
            }
            else if (i >= targetCount && i < currentCount)
            {
                indicators[i].gameObject.SetActive(true);
                indicators[i].color = Color.red;
            }
            else if (i < targetCount && i >= currentCount)
            {
                indicators[i].gameObject.SetActive(true);
                indicators[i].color = Color.white;
            }
            else if (i < targetCount && i < currentCount)
            {
                indicators[i].gameObject.SetActive(true);
                indicators[i].color = Color.green;
            }
        }
    }
}
