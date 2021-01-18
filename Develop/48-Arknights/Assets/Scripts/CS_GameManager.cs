using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour {

    private static CS_GameManager instance = null;
    public static CS_GameManager Instance { get { return instance; } }

    [SerializeField] int myMaxLife = 10;
    private int myCurrentLife;

    [SerializeField] List<CS_Player> myPlayerList = new List<CS_Player> ();

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start () {
        myCurrentLife = myMaxLife;
        CS_UIManager.Instance.SetLife (myCurrentLife);
    }

    public void LoseLife () {
        myCurrentLife--;
        CS_UIManager.Instance.SetLife (myCurrentLife);
    }

    public List<CS_Player> GetPlayerList () {
        return myPlayerList;
    }
}
