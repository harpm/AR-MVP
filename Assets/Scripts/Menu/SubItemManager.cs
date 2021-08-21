using System.Runtime.InteropServices;
using UnityEngine;

public class SubItemManager : MonoBehaviour
{
    [SerializeField]
    private SubItem prefabItem;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddSubItem(ArStuff stuff, string Name, [Optional] Sprite icon)
    {
        var item = Instantiate<SubItem>(prefabItem, transform);
        item.Name.text = Name;
        item.stuff = stuff;
        item.AddActionToButton();
        if (icon != null)
            item.Icon.sprite = icon;
    }
}
