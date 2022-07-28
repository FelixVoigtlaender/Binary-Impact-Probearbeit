using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class LevelSettings 
{
    public int currentIndex = 0;
    [HideInInspector]
    public UnityEvent onSpawnedLast;
    public bool isRandom = true;
    [NonReorderable]public List<TubeSpawnOrder> tubeSpawnOrders = new List<TubeSpawnOrder>();

}

[System.Serializable]
public class TubeSpawnOrder
{
    public Tube tube;
    public GameObject ballPrefab;
}