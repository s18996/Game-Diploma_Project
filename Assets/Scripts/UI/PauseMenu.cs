using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    [SerializeField] GameObject panel;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject resumeButton;

    public void Show() {
        panel.SetActive(true);
        exitButton.SetActive(true);
        restartButton.SetActive(true);
        resumeButton.SetActive(true);
    }

    public void Hide() {
        panel.SetActive(false);
        exitButton.SetActive(false);
        restartButton.SetActive(false);
        resumeButton.SetActive(false);
    }

}
