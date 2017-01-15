using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosersDialog : MonoBehaviour {

    public Button RestartButton;

    void Start() {
        RestartButton.onClick.AddListener(RestartLevel);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

}
