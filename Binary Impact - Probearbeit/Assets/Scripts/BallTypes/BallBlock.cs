using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBlock : Ball
{
    public override bool CanExit(Tube tube)
    {
        return false;
    }
}
