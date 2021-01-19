using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Player_Healer : CS_Player {

    [Header ("Healer")]
    private CS_Player myTargetPlayer;

    protected override void Update_Attack () {
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

                // if the player is full health, dont set it as target
                if (f_player.GetHealthPercent () >= 1) {
                    continue;
                }

                if (CheckInRange (f_player.transform) == true) {
                    myTargetPlayer = f_player;
                    break;
                }
            }
        }

        // if no enemy in range, dont attack
        if (myTargetPlayer == null) {
            return;
        }

        // if the player is full health, dont set it as target
        if (myTargetPlayer.GetHealthPercent () >= 1) {
            myTargetPlayer = null;
            return;
        }

        // if the enemy move out of the range, stop attacking this enemy
        if (CheckInRange (myTargetPlayer.transform) == false) {
            myTargetPlayer = null;
            return;
        }

        // play sfx
        myAudioSource_Attack.Play ();

        // play effect
        myEffect.Kill ();
        myEffect.transform.position = myTargetPlayer.transform.position;
        myEffect.gameObject.SetActive (true);

        // attack enemy
        myTargetPlayer.TakeDamage (myStatus_Attack * -1);
        myAttackTimer += myStatus_AttackTime;
        myAnimator.SetTrigger ("Attack");
    }
}
