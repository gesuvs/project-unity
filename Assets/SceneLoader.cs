using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    private bool _isSceneLoaded = false;


    public GameObject loadingScreen;
    public Image loadingBarFill;

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        var operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            var progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
    
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }


}
