using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AnswerLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    private bool isVisible;

    public void AppendTestLine(Vector3 position)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, position);
    }

    public void ClearLine()
    {
        line.positionCount = 0;
    }

    public void ToggleActiveState()
    {
        isVisible = !isVisible;
        line.enabled = isVisible;
    }

    public void HideLine()
    {
        isVisible = false;
        line.enabled = false;
    }
}
