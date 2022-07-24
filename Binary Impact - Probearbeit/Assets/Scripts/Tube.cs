using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro.EditorUtilities;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public List<Ball> balls = new List<Ball>();
    public Transform entry;
    public Transform exit;
    public float ballRadius = 0.5f;
    public float exitDelay = 0.3f;
    public int maxAmount;

    public Colored colored;
    
    private void Start()
    {
        if (colored)
            SetColor(colored.color);
        
        // Calc max amount for tube
        Vector3 difference =exit.position - entry.position;
        maxAmount = Mathf.FloorToInt((difference.magnitude / (ballRadius*2)));

        StartCoroutine(HandleTubeExit());
    }

    public void SetColor(Color color)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        color.a = spriteRenderer.color.a;

        spriteRenderer.color = color;
    }

    private IEnumerator HandleTubeExit()
    {
        while (true)
        {
            yield return new WaitForSeconds(exitDelay);
            if (balls.Count == 0)
                continue;

            Ball ball = balls[0];
            if (!ball.CanExit(this))
                continue;
            
            ball.ExitTube();
        }
    }

    public void Clear()
    {
        foreach (var ball in balls)
        {
            Destroy(ball.gameObject);
        }
        balls.Clear();
    }


    public bool CanEnter()
    {
        return maxAmount > balls.Count;
    }
    
    public void Enter(Ball ball)
    {
        // Double reference
        if(balls.Contains(ball))
            return;

        // adding ball at last point
        balls.Add(ball);
        
        HandleBallPositions();
    }

    public void Insert(Ball ball, int index)
    {
        if(index >= maxAmount || index > balls.Count)
            return;

        balls[index] = ball;
        
        HandleBallPositions();
    }

    public Vector3 GetBallPosition(int index)
    {
        if (!entry || !exit)
            return transform.position;

        Vector3 difference =entry.position - exit.position;
        float stepDistance = ballRadius * 2;

        float distance = index * stepDistance;
        Vector3 position = exit.position + distance * difference.normalized + difference.normalized * ballRadius;

        return position;
    }

    public int GetIndexOfBall(Ball ball)
    {
        if (!balls.Contains(ball))
            return -1;

        return balls.IndexOf(ball);
    }

    public void KillAtIndex(int index)
    {
        if(index>=balls.Count)
            return;

        Ball ball = balls[index];
        balls.Remove(ball);
        Destroy(ball.gameObject);
        
        HandleBallPositions();
    }
    
    public void HandleBallPositions()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Vector3 position = GetBallPosition(i);
            balls[i].SetPosition(position);
        }
    }

    public void Exit(Ball ball)
    {
        balls.Remove(ball);
        HandleBallPositions();
        
        
        Vector3 difference =exit.position - entry.position;
        ball.SetPosition(exit.position + difference.normalized);
    }
}
