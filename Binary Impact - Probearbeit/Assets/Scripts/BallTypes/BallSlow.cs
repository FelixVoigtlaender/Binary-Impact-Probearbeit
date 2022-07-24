using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallSlow : BallColored
{
    public float slowdownTime = 3;
    
    
    public override void EnterTube(Tube tube)
    {
        base.EnterTube(tube);        
        BallManager.instance.SetFillingTime(slowdownTime,BallManager.instance.spawnDelay * 2f);
        transform.DOShakeScale(slowdownTime);
    }
}
