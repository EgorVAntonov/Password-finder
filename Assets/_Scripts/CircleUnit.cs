using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CircleUnit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public static CircleUnit lastTouchedCircle = null;
    [SerializeField] private ChainBuilder chain;
    [SerializeField] private DoubleClickChecker doubleClick;

    [SerializeField] private SpriteRenderer arrowToPrevCircle;
    [SerializeField] private SpriteRenderer arrowToNextCircle;
    [SerializeField] private Vector3 defaultHintPosition;

    public Vector3 unitPosition;
    public Vector2 unitIndexes;
    public bool isInChain;
    public bool arrowRevealed;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        isInChain = false;
        chain = FindObjectOfType<ChainBuilder>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerId != 0) return;
        if (lastTouchedCircle == this) return;

        chain.HandleUnitTouch(this);
    }

    public void SetLastTouchedCircle()
    {
        Debug.Log(gameObject.name + " become an active circle");
        lastTouchedCircle = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId != 0) return;

        if (doubleClick.Check())
        {
            Debug.Log("double click");
            chain.ClearConnections(this);
        }
    }

    public void SetUnitPosition()
    {
        unitPosition = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    #region hintsMethods
    public void SetHintSegmentToPrevCircle(Quaternion rotation, Color color)
    {
        arrowToPrevCircle.color = color; 
        arrowToPrevCircle.transform.parent.rotation = rotation;
        arrowToPrevCircle.transform.parent.gameObject.SetActive(false);
    }

    public void SetHintSegmentToNextCircle(Quaternion rotation, Color color)
    {
        arrowToNextCircle.color = color;
        arrowToNextCircle.transform.parent.rotation = rotation;
        arrowToNextCircle.transform.parent.gameObject.SetActive(false);
    }

    public void ShowSegmentToPrevCircle()
    {
        arrowToPrevCircle.transform.parent.gameObject.SetActive(true);
        arrowToPrevCircle.transform.localPosition = defaultHintPosition;
        CheckIfSegmentIsBehindAnotherSegment(arrowToPrevCircle.transform, arrowToNextCircle.transform);
    }

    public void ShowSegmentToNextCircle()
    {
        arrowRevealed = true;
        arrowToNextCircle.transform.parent.gameObject.SetActive(true);
        arrowToNextCircle.transform.localPosition = defaultHintPosition;
        CheckIfSegmentIsBehindAnotherSegment(arrowToNextCircle.transform, arrowToPrevCircle.transform);
    }

    private void CheckIfSegmentIsBehindAnotherSegment(Transform first, Transform second)
    {
        if (second.parent.gameObject.activeSelf == false) return;

        if (first.parent.rotation != second.parent.rotation) return;

        first.transform.localPosition = defaultHintPosition + new Vector3(0f, -0.13f, 0f);
    }

    public void HideSegments()
    {
        if (arrowToPrevCircle != null)
        {
            arrowToPrevCircle.transform.parent.gameObject.SetActive(false);
        }
        if (arrowToNextCircle != null)
        {
            arrowToNextCircle.transform.parent.gameObject.SetActive(false);
        }
        arrowRevealed = false;
    } 
    #endregion
}
