using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [SerializeField]
    public Button button;

    [SerializeField]
    public TextMeshProUGUI Name;

    public SubItemManager subItems;

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (subItems != null)
                StartCoroutine(ToggleSubItems());
        });
    }

    private IEnumerator ToggleSubItems()
    {
        MenuManager.Instance.categoriesContentSizeFitter.enabled = false;
        subItems.gameObject.SetActive(!subItems.gameObject.activeSelf);
        yield return new WaitForEndOfFrame();
        MenuManager.Instance.categoriesContentSizeFitter.enabled = true;
    }
}
