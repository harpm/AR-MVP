using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    [SerializeField]
    public Button Button;

    [SerializeField]
    public TextMeshProUGUI Name;

    [SerializeField]
    public Material Material;

    [SerializeField]
    private Image Icon;

    [HideInInspector] public ArStuff.Layer ongoingLayer;


    void Start()
    {
        ApplyMaterial();
    }

    private void ApplyMaterial()
    {
        if (Material == null)
            Debug.Log("No material!");

        Icon.material = Material;
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("Layer: " + ongoingLayer);
        ArSceneManager.Instance.SelectedStuff.ChangeMaterial(Material, ongoingLayer);
    }

}
