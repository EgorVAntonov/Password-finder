using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class CircleUnit : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private ChainBuilder chain;

    [SerializeField] private TMP_Text numberText;

    public Vector3 unitPosition;
    public Vector2 unitIndexes;

    public bool isOccupied;
    public bool numberRevealed;

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

    public void SetUnitPosition()
    {
        unitPosition = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void SetNumber(int number)
    {
        numberText.text = number.ToString();
    }

    public void ShowNumber()
    {
        numberText.gameObject.SetActive(true);
        numberRevealed = true;
    }

    public void HideNumber()
    {
        numberText.gameObject.SetActive(false);
        numberRevealed = false;
    }

}
