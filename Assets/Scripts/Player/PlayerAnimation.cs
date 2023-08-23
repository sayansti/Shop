using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; }
    
    [SerializeField] private Animator animator;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Carry = Animator.StringToHash("Carry");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int CarryWalk = Animator.StringToHash("CarryWalk");

    [HideInInspector] public bool currentWalkState = false;
    private bool _currentIdleState = true;
    private bool _currentCarryState = false;
    [HideInInspector] public bool currentCarryWalkState = false;
    
    private void Awake() {
        
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    
    private void SetAnimationState(bool walk, bool idle, bool carry, bool carryWalk) {
        
        if (walk != currentWalkState) {
            
            animator.SetBool(Walk, walk);
            currentWalkState = walk;
        }

        if (idle != _currentIdleState) {
            
            animator.SetBool(Idle, idle);
            _currentIdleState = idle;
        }

        if (carry != _currentCarryState) {
            
            animator.SetBool(Carry, carry);
            _currentCarryState = carry;
        }

        if (carryWalk == currentCarryWalkState) return;
        animator.SetBool(CarryWalk, carryWalk);
        currentCarryWalkState = carryWalk;
    }

    private void PlayerWalk() => SetAnimationState(true, false, false, false);

    private void PlayerIdle() => SetAnimationState(false, true, false, false);
    
    private void PlayerCarry() => SetAnimationState(false, false, true, false);

    private void PlayerCarryWalk() => SetAnimationState(false, false, false, true);

    public void SetWalk() {
        
        var carrying = GameManager.Instance.Carrying;

        switch (carrying) {
            
            case true when !currentCarryWalkState:
                PlayerCarryWalk();
                break;
            case false when !currentWalkState:
                PlayerWalk();
                break;
        }
    }

    public void SetIdle() {
        
        var carrying = GameManager.Instance.Carrying;

        switch (carrying) {
            
            case true when !_currentCarryState:
                PlayerCarry();
                break;
            case false when !_currentIdleState:
                PlayerIdle();
                break;
        }
    }
}
