
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    
    public Colored[] coloreds;
    public static BallManager instance;
    public GameObject[] ballPrefabs;
    public float spawnDelay = 1f;
    public float currentSpawnDelay=  0.1f;

    private List<Tube> _tubes = new List<Tube>();
    private Coroutine spawningCoroutine;

    public LevelSettings spawnSettings;


    private bool canSpawn = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _tubes = new List<Tube>(FindObjectsOfType<Tube>());
        currentSpawnDelay = spawnDelay;


    }

    public void StartSpawning()
    {
        if(spawningCoroutine!=null)
            StopSpawning();
        
        spawningCoroutine = StartCoroutine(HandleBallSpawning());
    }

    public void StopSpawning()
    {
        if(spawningCoroutine == null)
            return;
        
        StopCoroutine(spawningCoroutine);
        spawningCoroutine = null;
    }

    public bool TrySpawnBall()
    {
        if (spawnSettings.isRandom)
        {
            
            Tube tube = FindValidRandomTube();
            
            print($"Found tube {tube}");
            if(!tube)
                return false;
            
            
            GameObject ballPrefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

            GameObject ballObj = Instantiate(ballPrefab, tube.entry.position, Quaternion.identity);
            Ball ball = ballObj.GetComponent<Ball>();
            ball.EnterTube(tube);
        }
        else
        {
            if (spawnSettings.tubeSpawnOrders.Count == 0 || spawnSettings.currentIndex >= spawnSettings.tubeSpawnOrders.Count)
            {
                return true;
            }

            TubeSpawnOrder spawnOrder = spawnSettings.tubeSpawnOrders[spawnSettings.currentIndex];
            Tube tube = spawnOrder.tube;
            GameObject ballPrefab =spawnOrder.ballPrefab;
            if (!tube.CanEnter())
                return false;

            GameObject ballObj = Instantiate(ballPrefab, tube.entry.position, Quaternion.identity);
            Ball ball = ballObj.GetComponent<Ball>();
            ball.EnterTube(tube);
            spawnSettings.currentIndex ++;

            if (spawnSettings.tubeSpawnOrders.Count ==  spawnSettings.tubeSpawnOrders.Count - 1)
            {
                spawnSettings.onSpawnedLast?.Invoke();
                return true;
            }
        }
        

        return true;
    }

    public Tube FindTubeByColored(Colored colored)
    {
        foreach (var tube in _tubes)
        {
            if (tube.colored == colored)
                return tube;
        }
        return null;
    }

    private IEnumerator HandleBallSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnDelay);
            if(!canSpawn)
                continue;

            TrySpawnBall();

        }
    }

    public int BallCount()
    {
        int ballCount = 0;
        foreach (var tube in _tubes)
        {
            ballCount += tube.balls.Count;
        }
        
        
        return ballCount;
    }
    
    public void Freeze(float duration)
    {
        if(!canSpawn)
            return;

        StartCoroutine(HandleFreeze(duration));
    }
    private IEnumerator HandleFreeze(float duration)
    {
        canSpawn = false;
        yield return new WaitForSeconds(duration);
        canSpawn = true;
    }

    public void SetFillingTime(float duration, float fillingTime)
    {
        StartCoroutine(HandleFillingTime(duration,fillingTime));
    }
    
    private IEnumerator HandleFillingTime(float duration, float fillingTime)
    {
        currentSpawnDelay = fillingTime;
        yield return new WaitForSeconds(duration);
        currentSpawnDelay = spawnDelay;
    }


    

    public Tube FindValidRandomTube()
    {
        Tube validTube = null;

        // Remove invalid Tubes
        List<Tube> tubes = new List<Tube>(_tubes.ToArray());
        for (int i = tubes.Count - 1; i >= 0 ; i--)
        {
            if (!tubes[i].CanEnter())
                tubes.Remove(tubes[i]);
        }

        // No valid Tubes available
        if (tubes.Count == 0)
            return null;
        
        // Get random Tube
        int randomIndex = Mathf.FloorToInt(Random.Range(0, (float) tubes.Count));
        validTube = tubes[randomIndex];
        


        return validTube;
    }
}
