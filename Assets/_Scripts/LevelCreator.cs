using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private GridCreator grid;
    [SerializeField] private AnswerLine answerLine;
    [SerializeField] private LevelValidator validator;
    [SerializeField] private HintsController hints;
    [SerializeField] private ChainBuilder builder;

    private List<CircleUnit> circlesChain;

    private void Start()
    {
        //CreateLevel();
    }

    public void CreateLevel()
    {
        PrepareCreating();
        grid.GenerateGrid();
        GenerateCircleChain();
        hints.SetCircleChain(circlesChain.ToArray());
        DefineSegmentKit();
        validator.SetupPresentors();
    }

    private void PrepareCreating()
    {
        SetEveryCircleFree();
        circlesChain = new List<CircleUnit>();
        validator.ClearTargetKit();
        answerLine.ClearLine();
        builder.ClearAllConnections();
    }

    private void SetEveryCircleFree()
    {
        if (circlesChain == null) return;

        foreach (var circle in circlesChain)
        {
            circle.isInChain = false;
        }
    }

    private void GenerateCircleChain()
    {
        CircleUnit randomUnit = grid.GetRandomFreeUnit();
        while (randomUnit != null)
        {
            if (circlesChain.Count > 0)
            {
                CircleUnit midUnit = grid.GetCircleBetween(circlesChain.ToArray()[circlesChain.Count - 1], randomUnit);
                while (midUnit != null)
                {
                    AppendCircleChain(midUnit);
                    midUnit = grid.GetCircleBetween(circlesChain.ToArray()[circlesChain.Count - 1], randomUnit);
                }
            }
            AppendCircleChain(randomUnit);
            randomUnit = grid.GetRandomFreeUnit();
        }

        answerLine.HideLine();
    }

    private void AppendCircleChain(CircleUnit nextUnit)
    {
        circlesChain.Add(nextUnit);
        nextUnit.isInChain = true;
        answerLine.AppendLine(nextUnit.unitPosition);
    }

    private void DefineSegmentKit()
    {
        CircleUnit[] circlesChainArray = circlesChain.ToArray();
        for (int i = 1; i < circlesChain.Count; i++)
        {
            Vector2 segmentOffset = circlesChainArray[i].unitIndexes - circlesChainArray[i - 1].unitIndexes;
            validator.AddToTargetKit(segmentOffset);
        }
    }
}
