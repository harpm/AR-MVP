using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubItem : MonoBehaviour
{
    [SerializeField()]
    public Image Icon;

    [SerializeField()]
    public TextMeshProUGUI Name;

    [SerializeField()]
    private Button button;

    public ArStuff stuff;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddActionToButton()
    {
        button.onClick.AddListener(ViewInAr);
    }

    private void ViewInAr()
    {
        ArSceneManager.Instance.curStuff = stuff;
        MenuManager.Instance.CloseMenu();
    }
}
