using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {

    [SerializeField] private Transform bar = null;
    [SerializeField] private Gradient gradient = null;

    public void SetHealth(float health) {
        bar.localScale = new Vector3(health, 1f);
        bar.GetComponentInChildren<SpriteRenderer>().color = gradient.Evaluate(health);
    }
}
