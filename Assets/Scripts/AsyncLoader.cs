using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{

    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;


    public void LoadLevelButton(int levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);


        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator LoadLevelAsync(int levelToLoad)
    {
        var operation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!operation.isDone)
        {
            var progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingSlider.value = progressValue;
            yield return null;
        }
    }
    
}
