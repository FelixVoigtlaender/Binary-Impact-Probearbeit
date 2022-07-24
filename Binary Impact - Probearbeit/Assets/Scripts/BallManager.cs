
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    
    public Colored[] coloreds;
    public static BallManager instance;
    public GameObject[] ballPrefabs;
    public float spawnDelay = 1f;
    public float currentSpawnDelay=  0.1f;

    private List<Tube> _tubes = new List<Tube>();


    private bool canSpawn = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _tubes = new List<Tube>(FindObjectsOfType<Tube>());
        currentSpawnDelay = spawnDelay;


        StartCoroutine(HandleBallSpawning());
    }

    private IEnumerator HandleBallSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnDelay);
            if(!canSpawn)
                continue;
            

            Tube tube = FindValidTube();
            
            print($"Found tube {tube}");
            if(!tube)
                continue;

            GameObject ballPrefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

            GameObject ballObj = Instantiate(ballPrefab, tube.entry.position, Quaternion.identity);
            Ball ball = ballObj.GetComponent<Ball>();
            ball.EnterTube(tube);
        }
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


    

    public Tube FindValidTube()
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
