using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private bool isGameOver = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
     gameOverPanel.SetActive(false);
     restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

	    if (!isGameOver)
	    {
		    isGameOver = true;

		    StartCoroutine(GameOverSequence());
	    }
	    
    }

    private IEnumerator GameOverSequence()
    {
	    gameOverPanel.SetActive(true);

	    yield return new WaitForSeconds(5.0f);
	    
	    restartButton.gameObject.SetActive(true);

    }
    
    
}
