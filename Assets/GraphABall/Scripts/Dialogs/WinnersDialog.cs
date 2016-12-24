using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnersDialog : MonoBehaviour {

    public string NextLevelName;
    public Button NextButton;

    void Start() {
        NextButton.onClick.AddListener(ChangeLevel);
    }

    public void ChangeLevel() {
        SceneManager.LoadScene(NextLevelName, LoadSceneMode.Single);
    }

}
