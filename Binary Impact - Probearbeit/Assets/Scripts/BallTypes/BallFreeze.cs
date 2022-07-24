using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFreeze : BallColored
{
    private float freezeDuration = 3;
    public override void EnterTube(Tube tube)
    {
        base.EnterTube(tube);

        BallManager.instance.Freeze(freezeDuration);
    }
}
