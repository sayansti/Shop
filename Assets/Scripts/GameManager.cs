using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour  {
    
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform clothHolder;
    private MeshRenderer _clothMeshRenderer;

    public float timer;

    private List<Color> _clothesNeed = new();

    [SerializeField] private Text displayText;
    private StringBuilder _displayTextBuilder;

    public bool Carrying { get; private set; }
    private int ClothesCarrying { get; set; }

    private void Awake() {
        
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        _clothMeshRenderer = clothHolder.GetComponent<MeshRenderer>();
        _clothMeshRenderer.enabled = false;
        _displayTextBuilder = new StringBuilder();
    }

    public void SetClothColor(Color color) {
        
        if (!Carrying) {
            
            _clothMeshRenderer.enabled = true;
            _clothMeshRenderer.material.color = color;
            PlayerAnimation.Instance.SetWalk();
            Carrying = true;
            ClothesCarrying++;
        }
        else {
            
            var scale = clothHolder.localScale;
            scale.z += 0.5f;
            clothHolder.localScale = scale;
            ClothesCarrying++;
        }
    }

    public void SetClothRequirement() {
        
        _clothesNeed.Add(Color.green);
        _displayTextBuilder.Append("Need <color=green>Dress</color>\n");
        displayText.text = _displayTextBuilder.ToString();
    }

    public void GiveItem() {
        
        if (!Carrying) return;

        var scale = clothHolder.localScale;
        scale.z -= 0.5f;
        ClothesCarrying--;
        clothHolder.localScale = scale;
        _displayTextBuilder.Append("Received <color=green>Dress</color>\n");
        displayText.text = _displayTextBuilder.ToString();

        if (ClothesCarrying != 0) return;
        _clothMeshRenderer.enabled = false;
        Carrying = false;
    }
}
