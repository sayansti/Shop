using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck: MonoBehaviour {
    
    private Dictionary<string, Color> _rack;
    
    private void Start() {
        
        _rack = new Dictionary<string, Color> {
            
            { "Rack0", Color.blue },
            { "Rack1", Color.green },
            { "Rack2", Color.red }
        };
    }

    private void OnTriggerEnter(Collider other) {
        
        var isRack = other.tag.Equals("Rack");
        var isCustomer = other.tag.Equals("Customer");
        
        if (isRack)
            GameManager.Instance.SetClothColor(_rack[other.tag + other.transform.GetSiblingIndex()]);
        else if (isCustomer)
            CustomerMovement.Instance.ProductReceived(other.transform.GetSiblingIndex());
    }

    private void OnTriggerStay(Collider other) {
        
        if (!other.tag.Equals("Rack")) return;
        GameManager.Instance.timer += Time.deltaTime;

        if (GameManager.Instance.timer < 1) return;
        GameManager.Instance.SetClothColor(_rack[other.tag + other.transform.GetSiblingIndex()]);
        GameManager.Instance.timer = 0;
        PlayerAnimation.Instance.SetIdle();
    }

    private void OnTriggerExit(Collider other) {
        
        GameManager.Instance.timer = 0;
    }
}
