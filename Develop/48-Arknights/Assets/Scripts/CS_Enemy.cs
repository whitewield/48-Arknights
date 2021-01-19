using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Enemy : MonoBehaviour {
    enum State {
        Move = 0,
        Attack = 1,
        Dead = 9,
    }

    [SerializeField] SpriteRenderer mySpriteRenderer;
    private State myState;
    private List<Vector3> myPath;
    [SerializeField] float myMoveSpeed = 10;
    [SerializeField] Animator myAnimator = null;

    [SerializeField] Transform myTransform_HPBar = null;
    [SerializeField] GameObject myObject_HPCanvas = null;

    private CS_Player myTargetPlayer;

    [Header ("Status")]
    [SerializeField] int myStatus_MaxHealth = 1000;
    private int myCurrentHealth;
    [SerializeField] int myStatus_Attack = 200;
    [SerializeField] float myStatus_AttackTime = 0.5f;
    private float myAttackTimer = 0;


    public void Init () {
        // get path
        myPath = CS_EnemyManager.Instance.GetPath ();
        // move to the start point
        this.transform.position = myPath[0];
        myPath.RemoveAt (0);
        // set state to move
        myState = State.Move;
        myAnimator.SetInteger ("State", 0);

        // init health
        myCurrentHealth = myStatus_MaxHealth;
        myTransform_HPBar.localScale = Vector3.one;
        myObject_HPCanvas.SetActive (false);

        // active the enemy
        this.gameObject.SetActive (true);
    }

    private void FixedUpdate () {
        if (myState == State.Move) {
            Update_Move ();
        }

        Update_Attack ();
    }

    private void Update_Attack () {

        // update attack timer
        if (myAttackTimer > 0) {
            myAttackTimer -= Time.fixedDeltaTime;
            return;
        }

        // if enemy is gone, remove target
        if (myTargetPlayer != null && myTargetPlayer.gameObject.activeSelf == false) {
            myTargetPlayer = null;
        }

        // if i dont have a target, go through enemy list to find a target
        if (myTargetPlayer == null) {
            List<CS_Player> t_playerList = CS_GameManager.Instance.GetPlayerList ();
            foreach (CS_Player f_player in t_playerList) {
                if (f_player == null || f_player.gameObject.activeSelf == false ||
                    f_player.GetState () == CS_Player.State.Dead ||
                    f_player.GetState () == CS_Player.State.Arrange) {
                    continue;
                }
                if (Vector3.Distance(f_player.transform.position, this.transform.position) < 0.5f) {
                    myTargetPlayer = f_player;
                    break;
                }
            }
        }

        // if no enemy in range, dont attack
        if (myTargetPlayer == null) {
            if (myState != State.Dead) {
                myState = State.Move;
                myAnimator.SetInteger ("State", 0);
            }
            return;
        }

        // attack enemy
        myTargetPlayer.TakeDamage (myStatus_Attack);
        myAttackTimer += myStatus_AttackTime;
        myAnimator.SetTrigger ("Attack");
        myState = State.Attack;
        myAnimator.SetInteger ("State", 1);
    }

    public void Update_Move () {
        // make sure there is at least one target point
        if (myPath == null || myPath.Count <= 0) {
            // hide enemy
            this.gameObject.SetActive (false);
            // tell manager lose enemy
            CS_EnemyManager.Instance.LoseEnemy (this);
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

        // update animation
        // only flip if moving horizontally
        if (Mathf.Abs (t_direction.x) > Mathf.Abs (t_direction.y)) {
            if (t_direction.x > 0) {
                mySpriteRenderer.flipX = false;
            } else {
                mySpriteRenderer.flipX = true;
            }
        }
    }

    public void TakeDamage (int g_damage) {
        myCurrentHealth -= g_damage;

        if (myCurrentHealth <= 0) {
            myCurrentHealth = 0;
            // set dead
            myState = State.Dead;
            // hide enemy
            this.gameObject.SetActive (false);
            // tell manager lose enemy
            CS_EnemyManager.Instance.LoseEnemy (this);
        }

        // active the canvas
        myObject_HPCanvas.SetActive (true);
        // update HP bar ui
        myTransform_HPBar.localScale = new Vector3 ((float)myCurrentHealth / myStatus_MaxHealth, 1, 1);
    }
}
