using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Player : MonoBehaviour {
    public enum State {
        Idle = 0,
        Attack = 1,
        Dead = 9,
        Arrange = 10,
    }

    private State myState = State.Arrange;
    [SerializeField] Transform myRangeParent = null;
    [SerializeField] Transform myRotateTransform = null;
    private CS_Enemy myTargetEnemy;

    [SerializeField] protected Animator myAnimator = null;

    [SerializeField] Transform myTransform_HPBar = null;

    [SerializeField] protected GameObject myEffectPrefab = null;
    protected CS_Effect myEffect = null;

    [SerializeField] protected AudioSource myAudioSource_Attack;

    [Header ("Status")]
    [SerializeField] CS_Tile.Type myTileType = CS_Tile.Type.Ground;
    [SerializeField] int myStatus_MaxHealth = 2400;
    private int myCurrentHealth;
    [SerializeField] protected int myStatus_Attack = 700;
    [SerializeField] protected float myStatus_AttackTime = 0.5f;
    protected float myAttackTimer = 0;

    public void Arrange () {
        myState = State.Arrange;
    }

    public void Init () {
        myState = State.Idle;
        // hide highlight
        HideHighlight ();
        // face camera
        FaceCamera ();
        // init health
        myCurrentHealth = myStatus_MaxHealth;
        myTransform_HPBar.localScale = Vector3.one;
        // init effect
        if (myEffect == null) {
            myEffect = Instantiate (myEffectPrefab).GetComponent<CS_Effect> ();
            myEffect.Kill ();
        }
    }

    private void FixedUpdate () {
        Debug.Log (myState);
        if (myState == State.Arrange || myState == State.Dead) {
            return;
        }
        Update_Attack ();
    }

    private void Update () {
        if (myState == State.Arrange) {
            FaceCamera ();
        }
    }

    private void FaceCamera () {
        // face camera
        myRotateTransform.rotation = Quaternion.identity;
    }

    public void ShowHighlight () {
        for (int i = 0; i < myRangeParent.childCount; i++) {
            myRangeParent.GetChild (i).gameObject.SetActive (true);
        }
    }

    public void HideHighlight () {
        for (int i = 0; i < myRangeParent.childCount; i++) {
            myRangeParent.GetChild (i).gameObject.SetActive (false);
        }
    }

    protected virtual void Update_Attack () {
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
                if (CheckInRange (f_enemy.transform) == true) {
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
        if (CheckInRange (myTargetEnemy.transform) == false) {
            myTargetEnemy = null;
            Debug.Log ("out of range");
            return;
        }

        // play sfx
        myAudioSource_Attack.Play ();

        // play effect
        myEffect.Kill ();
        myEffect.transform.position = myTargetEnemy.transform.position;
        myEffect.gameObject.SetActive (true);

        // attack enemy
        myTargetEnemy.TakeDamage (myStatus_Attack);
        myAttackTimer += myStatus_AttackTime;
        myAnimator.SetTrigger ("Attack");
    }

    protected bool CheckInRange (Transform g_transform) {
        Vector3 t_position = g_transform.position;

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

        if (myCurrentHealth > myStatus_MaxHealth) {
            myCurrentHealth = myStatus_MaxHealth;
        }

        // update HP bar ui
        myTransform_HPBar.localScale = new Vector3 (GetHealthPercent (), 1, 1);
    }

    public float GetHealthPercent () {
        return (float)myCurrentHealth / myStatus_MaxHealth;
    }

    public CS_Tile.Type GetTileType () {
        return myTileType;
    }

    public State GetState () {
        return myState;
    }
}
