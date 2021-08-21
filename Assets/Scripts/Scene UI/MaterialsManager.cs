using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialsManager : MonoBehaviour
{
    public static MaterialsManager Instance;

    [SerializeField]
    private FieldButton _fieldButton;

    [SerializeField]
    private MaterialButton _materialButton;

    private MaterialFields previousFields;

    [SerializeField]
    private Animator sceneUIAnimator;
    
    [SerializeField]
    private GameObject content;

    public State curState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddFields(MaterialFields fields)
    {
        Clear();
        previousFields = fields;
        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Cloth) &&
            fields.Cloth != null &&
            fields.Cloth.Any())
        {
            AddField("Cloth", fields.Cloth, ArStuff.Layer.Cloth);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Cloth2) &&
            fields.Cloth2 != null &&
            fields.Cloth2.Any())
        {
            AddField("Cloth 2", fields.Cloth2, ArStuff.Layer.Cloth2);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Wood) &&
            fields.Wood != null &&
            fields.Wood.Any())
        {
            AddField("Wood", fields.Wood, ArStuff.Layer.Wood);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Wood2) &&
            fields.Wood2 != null &&
            fields.Wood2.Any())
        {
            AddField("Wood 2", fields.Wood2, ArStuff.Layer.Wood2);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Metal) &&
            fields.Metal != null &&
            fields.Metal.Any())
        {
            AddField("Metal", fields.Metal, ArStuff.Layer.Metal);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Plastic) &&
            fields.Plastic != null &&
            fields.Plastic.Any())
        {
            AddField("Plastic", fields.Plastic, ArStuff.Layer.Plastic);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Color) &&
            fields.Color != null &&
            fields.Color.Any())
        {
            AddField("Color", fields.Color, ArStuff.Layer.Color);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Other) &&
            fields.Other != null &&
            fields.Other.Any())
        {
            AddField("Other", fields.Other, ArStuff.Layer.Other);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Other2) &&
            fields.Other2 != null &&
            fields.Other2.Any())
        {
            AddField("Other 2", fields.Other2, ArStuff.Layer.Other2);
        }

        if (ArSceneManager.Instance.SelectedStuff.CheckLayer(ArStuff.Layer.Other3) &&
            fields.Other3 != null &&
            fields.Other3.Any())
        {
            AddField("Other 3", fields.Other3, ArStuff.Layer.Other3);
        }
    }

    private void AddField(string Name, List<Material> materials, ArStuff.Layer layer)
    {
        var field = Instantiate(_fieldButton, content.transform);
        field.Name.text = Name;
        field.Button.onClick.AddListener(() => FieldsOnClick(materials, layer));
    }

    private void FieldsOnClick(List<Material> materials, ArStuff.Layer layer)
    {
        Clear();

        foreach (Material m in materials)
        {
            AddMaterial(m, layer);
        }

        curState = State.Materials;
    }

    private void AddMaterial(Material material, ArStuff.Layer layer)
    {
        var mat = Instantiate(_materialButton, content.transform);
        mat.Name.text = material.name;
        mat.Material = material;
        mat.ongoingLayer = layer;
    }

    private void Clear()
    {
        int count = content.transform.childCount;

        if (count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(content.transform.GetChild(0).gameObject);
        }
    }

    public void Open()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (!enabled)
            enabled = true;

        StartCoroutine(PlayOpenAnimation());
        AddFields(ArSceneManager.Instance.SelectedStuff.Fields);
    }

    private void Close()
    {
        Clear();
        StartCoroutine(PlayCloseAnimation());
    }

    private IEnumerator PlayOpenAnimation()
    {
        SetOpenTrigger();
        yield return new WaitForSeconds(1);
        yield return new WaitForEndOfFrame();
        SetOpenTrigger();
    }

    private IEnumerator PlayCloseAnimation()
    {
        SetCloseTrigger();
        yield return new WaitForSeconds(0.6f);
        yield return new WaitForEndOfFrame();
        SetCloseTrigger();
    }

    private void SetOpenTrigger()
    {
        sceneUIAnimator.SetTrigger("OpenMaterials");
    }

    private void SetCloseTrigger()
    {
        sceneUIAnimator.SetTrigger("CloseMaterials");
    }

    public void Back()
    {
        switch (curState)
        {
            case State.None:
                {
                    MenuManager.Instance.OpenMenu();
                    break;
                }
            case State.Fields:
                {
                    Close();
                    curState = State.None;
                    break;
                }
            case State.Materials:
                {
                    AddFields(previousFields);
                    curState = State.Fields;
                    break;
                }
        }
    }

    public enum State
    {
        None = 0,
        Fields = 1,
        Materials = 2
    }
}
