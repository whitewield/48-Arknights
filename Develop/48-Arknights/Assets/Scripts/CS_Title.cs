using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_Title : MonoBehaviour {
    public void OnButtonStart () {
        SceneManager.LoadScene ("Game");
    }
}
