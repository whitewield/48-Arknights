using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Highlight : MonoBehaviour {
    [SerializeField] MeshRenderer myMeshRenderer;
    private Material myMaterial;


    void Start () {
        myMaterial = myMeshRenderer.material;
    }

    // Update is called once per frame
    void Update () {
        myMaterial.mainTextureOffset = new Vector2 (Time.time * 10, 0);
    }
}
