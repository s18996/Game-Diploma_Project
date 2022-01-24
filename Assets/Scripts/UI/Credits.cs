using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

    [SerializeField] Image backgroundColor;
    [SerializeField] GameObject button;
    [SerializeField] GameObject background;
    [SerializeField] GameObject movingText;
    public void Show() {
        background.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }

    IEnumerator ShowCoroutine() {
        for (float i = 0.01f; i <= 1; i += 0.01f) {
            backgroundColor.color = new Color(backgroundColor.color.r, backgroundColor.color.g, backgroundColor.color.b, i);
            yield return new WaitForSeconds(0.03f);
        }
        button.SetActive(true);
        movingText.SetActive(true);
    }

}
