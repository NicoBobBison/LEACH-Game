using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public CameraEffects camEffects;
    public bool inSceneTransition = false;
    public static bool gamePaused;

    public static SceneManagement instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gamePaused = false;
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        /*if (!PlayerPrefs.HasKey("coins"))
        {
            PlayerPrefs.SetInt("coins", 0);
        }*/
        GetSceneReferences();
        camEffects.PlaySceneTransition();
    }

    void Update()
    {
        if(camEffects == null)
        {
            GetSceneReferences();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            GetSceneReferences();
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        //CheckIfShouldTransition();
        if (Input.GetKeyDown(KeyCode.R))
        {
            camEffects.PlaySceneTransition(SceneManager.GetActiveScene().name);
        }
    }
    public void ChangeScene(string scene)
    {
        if (scene != SceneManager.GetActiveScene().name)
        {
            PlayerPrefs.SetString("previousScene", SceneManager.GetActiveScene().name);
        }
        if (Time.timeSinceLevelLoad < 0.03f)
        {
            Debug.LogWarning("Attempting to reload scene too quickly. Make sure the player's spawn and the area around it is unobstructed.");
        }
        GetSceneReferences();
        camEffects.PlaySceneTransition();
        SceneManager.LoadSceneAsync(scene);
        ResumeGame();
    }


    public void GetSceneReferences()
    {
        camEffects = GameObject.Find("Main Camera").GetComponent<CameraEffects>();
    }

    public GameObject FindSpawnPoint()
    {
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("LevelExit");
        foreach(GameObject respawn in respawns)
        {
            if(respawn.name.Equals(PlayerPrefs.GetString("previousScene")))
            {
                return respawn;
            }
        }
        Debug.LogWarning("Couldn't find respawn that matched scene name");
        return null;
    }
    public void ToMainMenu()
    {
        PlayerPrefs.SetString("currentScene", SceneManager.GetActiveScene().name);
        camEffects.PlaySceneTransition("MainMenu");
    }
    public void NewGame()
    {
        camEffects.PlaySceneTransition("Lab1");
    }
    public void ContinueGame()
    {
        camEffects.PlaySceneTransition(PlayerPrefs.GetString("currentScene"));
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
        camEffects.SetDimmerLevel(0.4f);
        camEffects.EnablePauseText();
    }
    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        GetSceneReferences();
        camEffects.SetDimmerLevel(0);
        camEffects.DisablePauseText();
    }
}
