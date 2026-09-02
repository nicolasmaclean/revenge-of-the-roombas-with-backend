using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    /// <summary>
    /// Goes to the next level. If there are no more levels, it will go the level 0.
    /// </summary>
    public void GoToNextLevel()
    {
        int nextBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextBuildIndex >= SceneManager.sceneCountInBuildSettings) nextBuildIndex = 0;
        GoToLevel(nextBuildIndex);
    }

    /// <summary>
    /// Goes to level 0.
    /// </summary>
    public void GoToStartMenu()
    {
        GoToLevel(0);
    }

    /// <summary>
    /// Goes to level <paramref name="buildIndex"/>
    /// </summary>
    /// <param name="buildIndex"> the build index of the scene to load. </param>
    public void GoToLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (FindObjectOfType<Game.Player.PlayerController>() == null)
                GoToNextLevel();
        }
    }
}