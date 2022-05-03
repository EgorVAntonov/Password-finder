using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleUnit : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private ChainBuilder chain;

    public Vector2 unitIndexes;

    public bool isOccupied;

    void Start()
    {
        chain = FindObjectOfType<ChainBuilder>();
        isOccupied = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOccupied) return;
        if (eventData.pointerId != 0) return;

        isOccupied = true;
        if (chain != null)
        {
            chain.HandleUnitTouch(this);
        }
    }
}
