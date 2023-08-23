using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomerMovement : MonoBehaviour {
    
    [SerializeField] private float moveSpeed = 5f;
    public static CustomerMovement Instance { get; private set; }
    
    [SerializeField] private List<Transform> targetPositions = new();
    private List<Transform> _customers = new();

    private bool _reached = false;
    
    private List<Animator> _animator;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Win = Animator.StringToHash("Win");

    private void Awake() {
        
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _animator = new List<Animator>();

        for (var i = 0; i < transform.childCount; i++)
        {
            var childTransform = transform.GetChild(i);
            _customers.Add(childTransform);
            _animator.Add(childTransform.GetChild(0).GetComponent<Animator>());
        }
    }

    private void Update() {
        
        MoveCustomer();
    }

    private void MoveCustomer() {

        if (_reached) return;
        
        for (var i = 0; i < _customers.Count; i++) {

            if (Vector3.Distance(_customers[0].position, targetPositions[0].position) < 1) {

                if (_reached) continue;
                
                _reached = true;
                for (var j = 0; j < _customers.Count; j++) {
                    
                    GameManager.Instance.SetClothRequirement();
                    SetAnimationState(j, false);
                }

                continue;
            }
            
            var newPosition = Vector3.MoveTowards(_customers[i].position, targetPositions[i].position, moveSpeed * Time.deltaTime);
            _customers[i].position = newPosition;
            HandleCustomerRotation(_customers[i], targetPositions[i].position);
            SetAnimationState(i, true);
        }
    }
    
    private void HandleCustomerRotation(Transform transform1, Vector3 targetPos) {
        
        var delta = targetPos - transform1.position;
        if (delta == Vector3.zero) return;

        var targetRotation = Quaternion.LookRotation(delta);
        transform1.rotation = Quaternion.Slerp(transform1.rotation, targetRotation, Time.deltaTime * 100f);
    }

    public void ProductReceived(int index)
    {
        SetAnimationState(index, false, true);
        GameManager.Instance.GiveItem();
    }
    
    private void SetAnimationState(int index, bool walk, bool win = false) {
        
        if (_animator[index].GetBool(Walk) == walk && _animator[index].GetBool(Win) == win) return;

        _animator[index].SetBool(Walk, walk);
        _animator[index].SetBool(Win, win);
    }
}
