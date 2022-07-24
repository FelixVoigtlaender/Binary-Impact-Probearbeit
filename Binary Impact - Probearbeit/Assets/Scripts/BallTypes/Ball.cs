using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;

public class Ball : MonoBehaviour
{
    public Tube tube;
    


    public virtual bool CanExit(Tube tube)
    {
        return true;
    }

    private void FixedUpdate()
    {
    }

    public virtual void EnterTube(Tube tube)
    {
        if(this.tube)
            this.tube.Exit(this);

        this.tube = tube;
        
        tube.Enter(this);
    }

    public void InsertTube(Tube tube, int index)
    {
        tube.Insert(this, index);
        this.tube = tube;
    }
    
    public void ExitTube()
    {
        if(!this.tube)
            return;
        
        tube.Exit(this);
        tube = null;
        
        Invoke(nameof(KillAfterExit), 1f);
    }

    public void KillAfterExit()
    {
        PointsManager.instance.DeltaPoints(1);
        GameObject.Destroy(gameObject);
    }

    public void SetPosition(Vector3 position)
    {
        transform.DOMove(position, 0.3f);
    }

    // Cause Collider is of Physics we need to do this workaround
    public virtual void CheckClick(Vector3 clickPosition)
    {
        Vector2 difference = clickPosition - transform.position;
        
        if (difference.magnitude > transform.lossyScale.x /2)
            return;
        
        Debug.DrawLine(clickPosition, transform.position, Color.green, 10);
        Selector.instance.OnSelect(this);
        
    }
}
