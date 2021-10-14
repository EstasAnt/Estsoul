using System.Collections;
using System.Collections.Generic;
using Character.Shooting;
using Game.Character.Melee;
using Game.LevelSpecial;
using UnityEngine;

public class GG_Attack_StateBehavioir : StateMachineBehaviour
{
    private MeleeWeapon _meleeWeapon;

    private bool _InputReceived = false;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_meleeWeapon == null)
            _meleeWeapon = animator.GetComponentInParent<WeaponController>().GetComponentInChildren<MeleeWeapon>();
        _meleeWeapon.CanReceiveInput = true;
        _meleeWeapon.CurrentAttackInComboChanged += MeleeWeaponOnCurrentAttackInComboChanged;
    }

    private void MeleeWeaponOnCurrentAttackInComboChanged(int obj)
    {
        if (!_InputReceived)
        {
            _InputReceived = true;
            _meleeWeapon.CanReceiveInput = false;
        }
    }
    
    // public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    // }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _meleeWeapon.CanReceiveInput = true;
        _InputReceived = false;
        _meleeWeapon.CurrentAttackInComboChanged -= MeleeWeaponOnCurrentAttackInComboChanged;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
