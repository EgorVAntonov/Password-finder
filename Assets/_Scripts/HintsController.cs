using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsController : MonoBehaviour
{
    private CircleUnit[] targetCircleChain;

    public void SetCircleChain(CircleUnit[] chain)
    {
        targetCircleChain = chain;
        for (int i = 0; i < targetCircleChain.Length; i++)
        {
            targetCircleChain[i].SetNumber(i + 1);
            targetCircleChain[i].HideNumber();
        }
    }

    public void ShowHint()
    {
        if (targetCircleChain.Length == 0) return;
        List<CircleUnit> units = GetUnitsWithUnrevealedNumber();
        if (units.Count == 0) return;

        units.ToArray()[Random.Range(0, units.Count)].ShowNumber();
    }

    private List<CircleUnit> GetUnitsWithUnrevealedNumber()
    {
        List<CircleUnit> units = new List<CircleUnit>();
        for (int i = 0; i < targetCircleChain.Length; i++)
        {
            if (targetCircleChain[i].numberRevealed == false)
            {
                units.Add(targetCircleChain[i]);
            }
        }
        return units;
    }
}
