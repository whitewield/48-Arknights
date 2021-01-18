using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EnemyManager : MonoBehaviour {

    private static CS_EnemyManager instance = null;
    public static CS_EnemyManager Instance { get { return instance; } }

    [SerializeField] GameObject myEnemyPrefab;
    [SerializeField] Transform[] myPathTransformArray;
    [SerializeField] float myPathRandomness = 0.2f;
    [SerializeField] float[] myEnemySpawnTimeArray;
    private float myTimer = -1;
    private int myEnemySpawnIndex = -1;
    private int myEnemyCount = -1;
    private List<CS_Enemy> myEnemyList = new List<CS_Enemy> ();

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start () {
        // init count
        myEnemyCount = 0;
        CS_UIManager.Instance.SetCount (myEnemyCount, myEnemySpawnTimeArray.Length);

        // generate enemy
        myTimer = myEnemySpawnTimeArray[0];
        myEnemySpawnIndex = 0;
    }

    private void Update () {
        Update_Timer ();
    }

    private void Update_Timer () {
        if (myTimer < 0) {
            return;
        }

        // update timer
        myTimer -= Time.deltaTime;
        if (myTimer > 0) {
            return;
        }

        // generate enemy
        GameObject t_enemyObject = Instantiate (myEnemyPrefab);
        t_enemyObject.SetActive (false);
        // get enemy script
        CS_Enemy t_enemy = t_enemyObject.GetComponent<CS_Enemy> ();
        // init enemy
        t_enemy.Init ();
        // add enemy to list
        myEnemyList.Add (t_enemy);

        // stop spawn process if all listed are finished
        myEnemySpawnIndex++;
        if (myEnemySpawnIndex >= myEnemySpawnTimeArray.Length) {
            myTimer = -1;
            return;
        }
        // set timer
        myTimer = myEnemySpawnTimeArray[myEnemySpawnIndex];
    }

    public List<Vector3> GetPath () {
        List<Vector3> t_pathList = new List<Vector3> ();
        foreach (Transform t_transform in myPathTransformArray) {
            // create some randomness for the path
            Vector3 f_position = t_transform.position;
            f_position.x = Random.Range (-myPathRandomness, myPathRandomness) + f_position.x;
            f_position.z = Random.Range (-myPathRandomness, myPathRandomness) + f_position.z;
            t_pathList.Add (f_position);
        }
        return t_pathList;
    }

    public void LoseEnemy (CS_Enemy g_enemy) {
        // update enemy count
        myEnemyCount++;
        CS_UIManager.Instance.SetCount (myEnemyCount, myEnemySpawnTimeArray.Length);

        // remove enemy from list
        myEnemyList.Remove (g_enemy);
    }

    public List<CS_Enemy> GetEnemyList () {
        return myEnemyList;
    }
}
