using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Player : MonoBehaviour {
    enum State {
        Idle,
        Attack,
        Dead,
    }

    private State myState;
    [SerializeField] Transform myRangeParent = null;
    [SerializeField] Transform myRotateTransform = null;
    private CS_Enemy myTargetEnemy;

    [SerializeField] Animator myAnimator = null;

    [SerializeField] Transform myTransform_HPBar = null;

    [Header ("Status")]
    [SerializeField] int myStatus_MaxHealth = 2400;
    private int myCurrentHealth;
    [SerializeField] int myStatus_Attack = 700;
    [SerializeField] float myStatus_AttackTime = 0.5f;
    private float myAttackTimer = 0;

    private void Start () {
        Init ();
    }

    public void Init () {
        // face camera
        myRotateTransform.rotation = Quaternion.identity;

        // init health
        myCurrentHealth = myStatus_MaxHealth;
        myTransform_HPBar.localScale = Vector3.one;
    }

    private void FixedUpdate () {
        Update_Attack ();
    }

    private void Update_Attack () {

        // update attack timer
        if (myAttackTimer > 0) {
            myAttackTimer -= Time.fixedDeltaTime;
            return;
        }

        // if enemy is gone, remove target
        if (myTargetEnemy != null && myTargetEnemy.gameObject.activeSelf == false) {
            myTargetEnemy = null;
        }

        // if i dont have a target, go through enemy list to find a target
        if (myTargetEnemy == null) {
            List<CS_Enemy> t_enemyList = CS_EnemyManager.Instance.GetEnemyList ();
            foreach (CS_Enemy f_enemy in t_enemyList) {
                if (CheckInRange (f_enemy) == true) {
                    myTargetEnemy = f_enemy;
                    break;
                }
            }
        }

        // if no enemy in range, dont attack
        if (myTargetEnemy == null) {
            return;
        }

        // if the enemy move out of the range, stop attacking this enemy
        if (CheckInRange (myTargetEnemy) == false) {
            myTargetEnemy = null;
            return;
        }

        // attack enemy
        myTargetEnemy.TakeDamage (myStatus_Attack);
        myAttackTimer += myStatus_AttackTime;
        myAnimator.SetTrigger ("Attack");
    }

    private bool CheckInRange (CS_Enemy g_enemy) {
        Vector3 t_position = g_enemy.transform.position;

        for (int i = 0; i < myRangeParent.childCount; i++) {
            Vector3 t_rangeCenter = myRangeParent.GetChild (i).position;
            if (t_position.x > t_rangeCenter.x - 0.5f && t_position.x < t_rangeCenter.x + 0.5f &&
                t_position.y > t_rangeCenter.y - 0.5f && t_position.y < t_rangeCenter.y + 0.5f) {
                return true;
            }
        }

        return false;
    }

    public void TakeDamage (int g_damage) {
        myCurrentHealth -= g_damage;

        if (myCurrentHealth <= 0) {
            myCurrentHealth = 0;
            // set dead
            myState = State.Dead;
            // hide enemy
            this.gameObject.SetActive (false);
        }

        // update HP bar ui
        myTransform_HPBar.localScale = new Vector3 ((float)myCurrentHealth / myStatus_MaxHealth, 1, 1);
    }
}
