using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClickChecker : MonoBehaviour
{
    [SerializeField] private float doubleClickBuffer;

    private float previousClick;

    void Start()
    {
        previousClick = Mathf.NegativeInfinity;
    }

    public bool Check()
    {
        if (Time.time - doubleClickBuffer < previousClick)
        {
            previousClick = Time.time;
            return true;
        }
        else
        {
            previousClick = Time.time;
        }
        return false;
    }
}
