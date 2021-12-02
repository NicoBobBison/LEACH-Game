using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour
{
    public Image dimmer;
    public bool transitioning = false;
    public SceneManagement sm { get; private set; }
    private void Awake()
    {
        dimmer = GameObject.Find("Dimmer").GetComponent<Image>();
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
        sm = GameObject.Find("Player").GetComponent<SceneManagement>();
        
    }
    
    public void PlaySceneTransition(string nextScene)
    {
        Debug.Log("start trans");
        if(dimmer.color.a < 0.1f)
        {
            Debug.Log("Dim");
            StartCoroutine(DimCam(nextScene));
        }
        else
        {
            Debug.Log("Brighten");
            StartCoroutine(BrightenCam());
        }
    }

    public void PlaySceneTransition()
    {
        StartCoroutine(BrightenCam());
    }

    public IEnumerator DimCam(string nextScene)
    {
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = 0;
        dimmer.color = tempColor;
        PlayerControls pc = GameObject.Find("Player").GetComponent<PlayerControls>();
        pc.StateMachine.ChangeState(pc.SceneTransState);
        while (dimmer.color.a < 1)
        {
            yield return new WaitForSeconds(0.005f);
            //Debug.Log(tempColor.a);
            tempColor.a += 0.02f;
            dimmer.color = tempColor;
        }
        SceneManagement.instance.ChangeScene(nextScene);
        transitioning = false;
    }
    public IEnumerator BrightenCam()
    {
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
        while (dimmer.color.a > 0)
        {
            yield return new WaitForSeconds(0.005f);
            tempColor.a -= 0.02f;
            dimmer.color = tempColor;
        }
        transitioning = false;
    }
}
