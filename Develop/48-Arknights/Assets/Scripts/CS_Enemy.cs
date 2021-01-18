using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Enemy : MonoBehaviour {
    enum State {
        Move,
        Attack,
    }

    private State myState;
    private List<Vector3> myPath;
    [SerializeField] float myMoveSpeed = 10;

    public void Init () {
        // get path
        myPath = CS_EnemyManager.Instance.GetPath ();
        // move to the start point
        this.transform.position = myPath[0];
        myPath.RemoveAt (0);
        // set state to move
        myState = State.Move;

        // active the enemy
        this.gameObject.SetActive (true);
    }

    private void FixedUpdate () {
        if (myState == State.Move) {
            Update_Move ();
        }
    }

    public void Update_Move () {
        // make sure there is at least one target point
        if (myPath == null || myPath.Count <= 0) {
            // hide enemy
            this.gameObject.SetActive (false);
            // tell manager lose enemy
            CS_EnemyManager.Instance.LoseEnemy ();
            // lose life
            CS_GameManager.Instance.LoseLife ();
            return;
        }

        // get current and target position
        Vector3 t_currentPosition = this.transform.position;
        Vector3 t_targetPosition = myPath[0];

        // calculate move direction
        Vector3 t_direction = (t_targetPosition - t_currentPosition).normalized;

        // move
        Vector3 t_nextPosition = this.transform.position + t_direction * myMoveSpeed * Time.fixedDeltaTime;

        // check if move over the target point
        Vector3 t_nextDirection = (t_targetPosition - t_nextPosition).normalized;
        if (t_nextDirection != t_direction) {
            // arrive
            t_nextPosition = t_targetPosition;
            // remove the point from list
            myPath.RemoveAt (0);
        }

        // set position
        this.transform.position = t_nextPosition;
    }
}
