using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUIManager : MonoBehaviour
{
    // UI Parts
    [SerializeField]
    private MaterialsManager _materialsManager;

    [SerializeField]
    private Button _deleteStuff;

    [SerializeField]
    private Button _changeMaterials;


    public static SceneUIManager Instance;
    private Page curPage;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Back()
    {
        switch (curPage)
        {
            case Page.Scene:
                {
                    MenuManager.Instance.OpenMenu();
                    break;
                }
            case Page.Materials:
                {
                    MaterialsManager.Instance.Back();
                    break;
                }
        }
    }

    private enum Page
    {
        Scene = 0,
        Materials = 1
    }
}
