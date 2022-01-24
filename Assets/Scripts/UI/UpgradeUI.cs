using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UpgradeUI : MonoBehaviour {

    [SerializeField] Sprite sword;
    [SerializeField] SpriteRenderer swordBase;
    Texture2D newTexture;
    private static string filePath = "Assets/Graphics/Sprites/Player/MySword.png";


    void Start() {
        newTexture = CloneTexture(sword.texture);
        SaveToFile(newTexture, filePath);
        //Texture2D loadedTexture = LoadTexture(filePath);
        //swordBase.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f), 32f);
    }

    //private void Update() {
    //    if (Input.GetMouseButtonDown(0)) {
    //        Texture2D loadedTexture = LoadTexture(filePath);
    //        swordBase.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f), 32f);
    //    }
    //}

    void SaveToFile(Texture2D texture, string filePath) {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }

    Texture2D LoadTexture(string filePath) {
        Texture2D texture;
        byte[] FileData;

        if (File.Exists(filePath)) {
            FileData = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2);
            if (texture.LoadImage(FileData))
                return texture;
        }
        return null;
    }

    Texture2D CloneTexture(Texture2D texture) {
        Texture2D newTexture = new Texture2D(32, 32);
        for (int i = 0; i < texture.GetPixels().Length; i++) {
            Color c = texture.GetPixels()[i];
            newTexture.SetPixel(i % 32, i / 32, new Color(c.r, c.g, c.b, c.a));
        }
        return newTexture;
    }

}
