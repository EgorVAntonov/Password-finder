using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  color version
 */
public class HintsController : MonoBehaviour
{
    private CircleUnit[] targetCircleChain;

    [SerializeField] private Color[] arrowsColors;
    private bool[] colorUsed = new bool[8];

    public void SetCircleChain(CircleUnit[] chain)
    {
        targetCircleChain = chain;
        if (targetCircleChain.Length == 0)
        {
            Debug.LogError("Circle chain for hints is 0 length");
            return;
        }
        SetupHints();
    }

    private void SetupHints()
    {
        ResetColorUsedFlags();

        Color nextColor = GetNextAvailableColorIndex();
        Quaternion nextRotation = GetRotation(targetCircleChain[0], targetCircleChain[1]);

        targetCircleChain[0].SetHintSegmentToNextCircle(nextRotation, nextColor);
        targetCircleChain[0].HideSegments();

        for (int i = 1; i < targetCircleChain.Length - 1; i++)
        {
            nextRotation = GetRotation(targetCircleChain[i], targetCircleChain[i-1]);
            targetCircleChain[i].SetHintSegmentToPrevCircle(nextRotation, nextColor);

            nextColor = GetNextAvailableColorIndex();

            nextRotation = GetRotation(targetCircleChain[i], targetCircleChain[i+1]);
            targetCircleChain[i].SetHintSegmentToNextCircle(nextRotation, nextColor);
            targetCircleChain[i].HideSegments();
        }
        nextRotation = GetRotation(targetCircleChain[targetCircleChain.Length - 1], targetCircleChain[targetCircleChain.Length - 2]);
        targetCircleChain[targetCircleChain.Length - 1].SetHintSegmentToPrevCircle(nextRotation, nextColor);
        targetCircleChain[targetCircleChain.Length - 1].HideSegments();
    }

    private void ResetColorUsedFlags()
    {
        for (int i = 0; i < colorUsed.Length; i++)
        {
            colorUsed[i] = false;
        }
    }

    private Color GetNextAvailableColorIndex()
    {
        int index = Random.Range(0, 8);
        for (int i = 0; i < 8; i++)
        {
            if (colorUsed[(index + i) % 8] == false)
            {
                colorUsed[(index + i) % 8] = true;
                return arrowsColors[(index + i) % 8];
            }
        }
        return Color.white;
    }

    private Quaternion GetRotation(CircleUnit from, CircleUnit to)
    {
        Vector3 downDirection = (to.unitPosition - from.unitPosition).normalized;
        Vector3 zDirection = Vector3.forward;
        return Quaternion.LookRotation(zDirection, downDirection);
    }

    public void ShowHint()
    {
        if (targetCircleChain.Length == 0) return;

        List<CircleUnit> units = GetUnitsWithUnrevealedSegments();
        if (units.Count == 0) return;

        //find in array
        CircleUnit currentUnit = units.ToArray()[Random.Range(0, units.Count)];
        for (int i = 0; i < targetCircleChain.Length; i++)
        {
            if (currentUnit != targetCircleChain[i]) continue;

            if (i > 0 && i < targetCircleChain.Length-1)
            {
                targetCircleChain[i].ShowSegmentToNextCircle();
                targetCircleChain[i+1].ShowSegmentToPrevCircle();
            }
            else if (i == 0)
            {
                targetCircleChain[0].ShowSegmentToNextCircle();
                targetCircleChain[1].ShowSegmentToPrevCircle();
            }
            else if (i == targetCircleChain.Length-1)
            {
                targetCircleChain[targetCircleChain.Length - 2].ShowSegmentToNextCircle();
                targetCircleChain[targetCircleChain.Length - 1].ShowSegmentToPrevCircle();
            }
        }
    }

    private List<CircleUnit> GetUnitsWithUnrevealedSegments()
    {
        List<CircleUnit> units = new List<CircleUnit>();
        for (int i = 0; i < targetCircleChain.Length - 1; i++)
        {
            if (targetCircleChain[i].arrowRevealed == false)
            {
                units.Add(targetCircleChain[i]);
            }
        }
        return units;
    }
}
