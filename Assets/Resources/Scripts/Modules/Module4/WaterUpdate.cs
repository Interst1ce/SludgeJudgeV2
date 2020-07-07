using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUpdate : MonoBehaviour {
    public int matIndex = 0;
    public float animSpeed = 1;

    string albedo = "_MainTex";
    string metallic = "_MetallicGlossMap";
    string normal = "_BumpMap";

    bool increasing;

    [SerializeField]
    Vector2 targetUVOffset = new Vector2(-0.1f,0.1f);
    Vector2 uvOffset = Vector2.zero;

    private void Update() {
        Renderer render = GetComponent<Renderer>();
        Vector2 albedoOffset = render.material.mainTextureOffset;

        if (albedoOffset.x < targetUVOffset.x) {
            increasing = true;
            targetUVOffset.x = Random.Range(-0.1f,0f) / 10;
        }
        if (albedoOffset.x > targetUVOffset.y) {
            increasing = false;
            targetUVOffset.y = Random.Range(0f,0.1f) / 10;
        }
    }

    private void LateUpdate() {
        Renderer render = GetComponent<Renderer>();
        Vector2 albedoOffset = render.material.mainTextureOffset;

        if (increasing) {
            uvOffset.x += animSpeed * Time.deltaTime;
            uvOffset.y += animSpeed * Time.deltaTime;
        } else {
            uvOffset.x -= animSpeed * Time.deltaTime;
            uvOffset.y -= animSpeed * Time.deltaTime;
        }

        if (render.enabled) {
            render.materials[matIndex].SetTextureOffset(albedo,uvOffset);
            render.materials[matIndex].SetTextureOffset(metallic,uvOffset);
            render.materials[matIndex].SetTextureOffset(normal,uvOffset);
        }
    }
}
