using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentPresentorsController : MonoBehaviour
{
    [SerializeField] private Transform _parent; 
    [SerializeField] private GameObject prefab;

    [SerializeField] private SegmentPresentor[] presentorsPool;

    [SerializeField] private int shuffleCount;
    private int presentorsCount;

    private void Start()
    {
        CreatePresentorsPool();
    }

    private void CreatePresentorsPool()
    {
        List<SegmentPresentor> pool = new List<SegmentPresentor>();
        for (int i = 0; i < 12; i++)
        {
            SegmentPresentor presentor = Instantiate(prefab, _parent).GetComponent<SegmentPresentor>();
            pool.Add(presentor);
            presentor.gameObject.SetActive(false);
        }
        presentorsPool = pool.ToArray();
    }

    public void CreatePresentors(List<Segment> segments)
    {
        presentorsCount = segments.Count;

        //TurnOffAllPresentors();

        Segment[] segmentsArray = segments.ToArray();
        for (int i = 0; i < presentorsPool.Length; i++)
        {
            if (i < presentorsCount)
            {
                presentorsPool[i].gameObject.SetActive(true);
                presentorsPool[i].Setup(segmentsArray[i]);
            }
            else
            {
                presentorsPool[i].gameObject.SetActive(false);
            }
        }

        ShufflePresentors();
    }

    private void TurnOffAllPresentors()
    {
        for (int i = 0; i < presentorsPool.Length; i++)
        {
            presentorsPool[i].gameObject.SetActive(false);
        }
    }

    private void ShufflePresentors()
    {
        for (int i = 0; i < shuffleCount; i++)
        {
            _parent.GetChild(Random.Range(0, presentorsCount)).SetAsFirstSibling();
        }
    }

    public void ShowCountMatch(SegmentType type, int targetCount, int currentCount)
    {
        for (int i = 0; i < presentorsCount; i++)
        {
            if (presentorsPool[i].segmentType == type)
            {
                presentorsPool[i].UpdateCountIndicators(targetCount, currentCount);
            }
        }
    }
}
