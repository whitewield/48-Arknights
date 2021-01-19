using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Tile : MonoBehaviour {
    public enum Type {
        Null = -1,
        Ground = 0,
        Step = 1,
    }

    [SerializeField] Type myType = Type.Ground;
    private CS_Player myPlayer;

    public Type GetType () {
        return myType;
    }

    public void OnClick () {
        if (myPlayer != null || myPlayer.gameObject.activeSelf == false) {
            CS_GameManager.Instance.OnClickTile (this);
        }
    }

    public void Occupy (CS_Player g_player) {
        myPlayer = g_player;
    }
}
