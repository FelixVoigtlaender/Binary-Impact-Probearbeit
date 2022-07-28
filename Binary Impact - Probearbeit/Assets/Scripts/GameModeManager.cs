using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameModeManager : MonoBehaviour
{
    
    [Header("GameModes")] 
    public AdventureSettings adventureSettings;
    public ArcadeSettings arcadeSettings;
    public ZenSettings zenSettings;
    
    public static GameModeManager instance;
    
    
    public IntHolder points;
    public IntHolder time;

    public CanvasGroup canvasGroup;


    public bool isInGame = false;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EndGame();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            EndGame();
    }


    public void PlayArcade()
    {
        StopAllCoroutines();
        StartCoroutine(HandleArcade());
    }
    public void PlayZen()
    {
        StopAllCoroutines();
        StartCoroutine(HandleZen());
    }
    public void PlayAdventure()
    {
        StopAllCoroutines();
        StartCoroutine(HandleAdventure());
    }
    
    
    
    void ResetGame()
    {
        points.value = 0;
        time.value = 0;
        BallManager.instance.StopSpawning();
        ClearAllTubes();
    }

    public void EndGame()
    {
        BallManager.instance.StopSpawning();
        StopAllCoroutines();
        
        // Handle Canvas Group
        canvasGroup.DOFade(1, 0.3f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        
        print("Game Ended");
    }

    void StartGame()
    {
        BallManager.instance.StartSpawning();
        
        // Handle Canvas Group
        canvasGroup.DOFade(0, 0.3f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    
    public void ClearAllTubes()
    {
        Tube[] tubes = FindObjectsOfType<Tube>();
        foreach (var tube in tubes)
        {
            tube.Clear();
        }
    }


    public IEnumerator HandleArcade()
    {
        ResetGame();
        BallManager.instance.spawnSettings = arcadeSettings.level;
        time.value = arcadeSettings.duration;
        
        StartGame();
        while (true)
        {
            yield return new WaitForSeconds(1);
            time.value -= 1;
            
            if(time.value == 0)
                break;
        }
        EndGame();
    }
    
    public IEnumerator HandleZen()
    {
        ResetGame();

        time.value =0;
        BallManager.instance.spawnSettings = zenSettings.level;
        
        StartGame();
        while (true)
        {
            yield return new WaitForSeconds(1);
            time.value += 1;
        }
    }
    
    public IEnumerator HandleAdventure()
    {
        int levelCount = 0;
        while (levelCount < adventureSettings.levels.Count)
        {
            ResetGame();
            time.value =0;
            LevelSettings level = adventureSettings.levels[levelCount];
            level.currentIndex = 0;
            BallManager.instance.spawnSettings = level;
            int currentLevel = levelCount;
            StartGame();
            while (currentLevel == levelCount)
            {
                yield return new WaitForSeconds(1);
                time.value += 1;

                if (BallManager.instance.BallCount() == 0)
                    levelCount++;

            }
        }
        EndGame();
    }



    [System.Serializable]
    public class ArcadeSettings
    {
        public int duration = 30;
        public LevelSettings level;
    }
    
    [System.Serializable]
    public class ZenSettings
    {
        public LevelSettings level;
    }
    
    [System.Serializable]
    public class AdventureSettings
    {
        [NonReorderable]public List<LevelSettings> levels = new List<LevelSettings>();
    }
}
