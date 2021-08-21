using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]

    [SerializeField]
    public ContentSizeFitter categoriesContentSizeFitter;

    [SerializeField]
    private Animator menuAnimator;

    [Header("Database")]

    [SerializeField]
    private List<StuffCategories> categories;

    [Header("Prefabs")]

    [SerializeField]
    private ListItem itemPrefab;

    [SerializeField]
    private SubItemManager subItemsPrefab;

    private Page curPage;

    public static MenuManager Instance;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
        }
        catch
        {
            Debug.LogWarning("Failed turning Vsync off!");
        }

        curPage = Page.Menu;

        Instance = this;

        for (int i = 0; i < categoriesContentSizeFitter.transform.childCount; i++)
        {
            Destroy(categoriesContentSizeFitter.transform.GetChild(i).gameObject);
        }

        CreateMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateMenu()
    {
        foreach (StuffCategories category in categories)
        {
            var cItem = Instantiate<ListItem>(itemPrefab, categoriesContentSizeFitter.transform);
            cItem.Name.text = category.Name;

            var sItems = Instantiate<SubItemManager>(subItemsPrefab, categoriesContentSizeFitter.transform);
            cItem.subItems = sItems;

            foreach (Stuff stuff in category.stuffs)
            {
                sItems.AddSubItem(stuff.Model, stuff.Name);
            }
        }
    }

    public void CloseMenu()
    {
        menuAnimator.SetTrigger("CloseMenu");
        curPage = Page.Scene;
    }

    public void OpenMenu()
    {
        menuAnimator.SetTrigger("OpenMenu");
        curPage = Page.Menu;
    }

    public void Back()
    {
        switch (curPage)
        {
            case Page.Menu:
                {
                    break;
                }
            case Page.Scene:
                {
                    SceneUIManager.Instance.Back();
                    break;
                }
            default:
                break;

        }
    }

    private enum Page
    {
        Menu = 0,
        Scene = 1
    }
}
