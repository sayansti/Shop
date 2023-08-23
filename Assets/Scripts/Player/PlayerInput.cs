using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    
    [SerializeField] private float moveSpeed = 10f;

    private Vector2 _touchStartPos;
    private bool _isTouching = false;
    private Rigidbody _rb;
    private Transform _childTransform;

    private void Start() {
        
        _rb = GetComponent<Rigidbody>();
        _childTransform = transform.GetChild(0);
    }

    private void Update() {
        
        if (Input.touchCount > 0) GetInput();
    }

    private void GetInput() {
        
        var touch = Input.GetTouch(0);

        switch (touch.phase) {
            
            case TouchPhase.Began:
                _touchStartPos = touch.position;
                _isTouching = true;
                break;
            case TouchPhase.Moved when _isTouching:
                HandleTouchMove(touch);
                break;
            case TouchPhase.Ended:
                HandleTouchEnd();
                break;
        }
    }
    
    private void HandleTouchMove(Touch touch) {
        
        var touchDelta = touch.position - _touchStartPos;
        var moveDirection = new Vector3(-touchDelta.x, 0f, -touchDelta.y).normalized;

        _rb.velocity = moveDirection * moveSpeed;
        
        if (!PlayerAnimation.Instance.currentWalkState || 
            !PlayerAnimation.Instance.currentCarryWalkState)
            PlayerAnimation.Instance.SetWalk();
        
        HandlePlayerRotation(moveDirection);
    }

    private void HandleTouchEnd() {
        
        _isTouching = false;
        _rb.velocity = Vector3.zero;
        PlayerAnimation.Instance.SetIdle();
    }

    private void HandlePlayerRotation(Vector3 moveDirection) {
        
        if (moveDirection == Vector3.zero) return;
        
        var targetRotation = Quaternion.LookRotation(moveDirection);
        _childTransform.rotation = Quaternion.Slerp(_childTransform.rotation, targetRotation, Time.deltaTime * 100f);
    }
}