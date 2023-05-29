using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadingScreen : MonoBehaviour // referenced this video for loading screen setup https://www.youtube.com/watch?v=wvXDCPLO7T0&ab_channel=SoloGameDev
{
    public GameObject loadingPanel;
    public Image loadingBarFill;
    
    public void LoadScene(int sceneId){
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingPanel.SetActive(true); // show panel while loading
        operation.allowSceneActivation = false;
        while(!operation.isDone){ // while loading
            float progressValue = Mathf.Clamp01(operation.progress/0.9f); // set progress value
            Debug.Log("progressValue: " + progressValue);
            loadingBarFill.fillAmount = progressValue;
            if (operation.progress >= 0.9f){
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
