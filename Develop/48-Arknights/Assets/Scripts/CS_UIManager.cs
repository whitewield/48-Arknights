using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CS_UIManager : MonoBehaviour {

    private static CS_UIManager instance = null;
    public static CS_UIManager Instance { get { return instance; } }
    // Start is called before the first frame update

    [SerializeField] Text myText_Life;
    [SerializeField] Text myText_Count;

    [SerializeField] GameObject myPage_End;
    [SerializeField] GameObject myPage_Fail;

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start () {
        myPage_End.SetActive (false);
        myPage_Fail.SetActive (false);
    }

    public void SetLife (int g_life) {
        myText_Life.text = g_life.ToString ();
    }

    public void SetCount (int g_current, int g_total) {
        myText_Count.text = g_current.ToString("0") + "/" + g_total.ToString ("0");
    }

    //public void OnButtonPlayer (int g_index) {
    //    CS_GameManager.Instance.SetMyCurrentPlayer (g_index);
    //}

    public void OnButtonTitle () {
        Time.timeScale = 1;
        SceneManager.LoadScene ("Title");
    }

    public void OnButtonRestart () {
        Time.timeScale = 1;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void ShowPageEnd () {
        myPage_End.SetActive (true);
    }

    public void ShowPageFail () {
        myPage_Fail.SetActive (true);
    }
}
