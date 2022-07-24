using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour, IPointerClickHandler
{
    public static Selector instance;
    
    public Transform from, to;

    public LineRenderer lineRenderer;


    public Ball firstSelection;
    public Ball secondSelection;

    private void Awake()
    {
        instance = this;
        HandleSelectionDrawing();
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
        
        Debug.DrawLine(position, Vector3.zero, Color.red, 10);

        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (var ball in balls)
        {
            ball.CheckClick(position); 
        }
    }


    public void OnSelect(Ball ball)
    {
        bool foundSelection = false;
        
        if (!foundSelection && ball == firstSelection)
        {
            firstSelection = null;
            foundSelection = true;
        }

        if (!foundSelection && ball == secondSelection)
        {
            secondSelection = null;
            foundSelection = true;
        }
        if (!foundSelection && !firstSelection)
        {
            firstSelection = ball;
            foundSelection = true;
        }
        if (!foundSelection && !secondSelection)
        {
            secondSelection = ball;
            foundSelection = true;
        }
        HandleSelectionDrawing();

        if (firstSelection && secondSelection)
        {
            HandleSwitch();
        }
    }


    public void HandleSwitch()
    {
        Tube firstTube = firstSelection.tube;
        Tube secondTube = secondSelection.tube;


        int firstIndex = firstTube.GetIndexOfBall(firstSelection);
        int secondIndex = secondTube.GetIndexOfBall(secondSelection);
        
        ;
        
        firstSelection.InsertTube(secondTube,secondIndex);
        secondSelection.InsertTube(firstTube, firstIndex);

        firstSelection = secondSelection = null;
        HandleSelectionDrawing();
    }
    
    
    public void HandleSelectionDrawing()
    {
        
        @from.gameObject.SetActive(true);
        to.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        
        if (firstSelection && secondSelection)
        {
            @from.position = firstSelection.transform.position;
            to.position = secondSelection.transform.position;
        }
        else if (firstSelection)
        {
            @from.position = to.position = firstSelection.transform.position;
        }else if (secondSelection)
        {
            @from.position = to.position = secondSelection.transform.position;
        }
        else
        {
            @from.gameObject.SetActive(false);
            to.gameObject.SetActive(false);
            lineRenderer.enabled = false;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, @from.position);
        lineRenderer.SetPosition(1, to.position);
    }

}
