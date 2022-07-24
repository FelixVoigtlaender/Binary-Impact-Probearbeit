using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BallBomb : BallColored
{
    public float tickingTime = 3;
    public SpriteRenderer explosionSprite;
    
    
    public override void EnterTube(Tube tube)
    {
        base.EnterTube(tube);
        if (explosionSprite)
            explosionSprite.enabled = false;
        
        StartCoroutine(HandleCountdown());
    }

    IEnumerator HandleCountdown()
    {
        transform.DOShakeScale(0.3f).SetLoops(-1);
        yield return new WaitForSeconds(tickingTime-0.1f);
        if (explosionSprite)
            explosionSprite.enabled = enabled;
        
        yield return new WaitForSeconds(0.1f);
        if(tube)
            tube.Clear();
        
        
    }
}
