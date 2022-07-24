using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallColored : Ball
{
    public Colored colored;

    private void Start()
    {
        
        Colored[] coloreds = BallManager.instance.coloreds;
        colored = coloreds[Random.Range(0, coloreds.Length)];
        
        SetColor(colored.color);
    }

    public void SetColor(Color color)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }


    public override bool CanExit(Tube tube)
    {
        return tube.colored.IsSame(colored);
    }
}
