using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_UIManager : MonoBehaviour {

    private static CS_UIManager instance = null;
    public static CS_UIManager Instance { get { return instance; } }
    // Start is called before the first frame update

    [SerializeField] Text myText_Life;
    [SerializeField] Text myText_Count;

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    public void SetLife (int g_life) {
        myText_Life.text = g_life.ToString ();
    }

    public void SetCount (int g_current, int g_total) {
        myText_Count.text = g_current.ToString("0") + "/" + g_total.ToString ("0");
    }
}
