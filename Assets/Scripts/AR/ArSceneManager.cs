using System.Collections.Generic;
using DigitalRubyShared;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSession))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ArStuffManager))]
public class ArSceneManager : MonoBehaviour
{
    public static ArSceneManager Instance;

    [SerializeField()]
    public Material onMovingMaterial;

    [SerializeField()]
    private Material arPlaneMaterial;

    [SerializeField()]
    public Material defaultMaterial;

    [SerializeField()]
    private Button ChangeMaterialBtn;

    [SerializeField()] 
    private Button DeleteStuffBtn;

    public ArStuff curStuff;
    public ArStuff SelectedStuff { get; private set; }

    // Cached Variables
    private ARPlaneManager arPlaneManager;
    private TapGestureRecognizer _tapGesture;

    // Ar Variables
    [HideInInspector()]
    public ARRaycastManager arRaycastManager;
    [HideInInspector()]
    public ARSession arSession;
    private ArStuffManager arStuffManager;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        arRaycastManager = GetComponent<ARRaycastManager>();
        arSession = GetComponent<ARSession>();
        arStuffManager = GetComponent<ArStuffManager>();
        CreatTapGesture();
        DisableControlStuffButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SelectStuff(ArStuff stuff)
    {
        Debug.Log("Selected");
        //DisplayPlane();
        SelectedStuff = stuff;
        SelectedStuff.Select();
        // selectedStuffLocalData = stuff.gameObject.GetComponent<StuffLocalData>();
        EnableControlStuffButtons();
    }

    public void DeleteStuff()
    {
        arStuffManager.Remove(SelectedStuff);
        var bk = SelectedStuff;
        DeselectSelectedStuff();
        Destroy(bk);
    }

    void DeselectSelectedStuff()
    {
        if (SelectedStuff == null)
            return;

        Debug.Log("Deselected");


        SelectedStuff.Deselect();
        //HidePlane();
        _tapGesture.StateUpdated -= TapGestureCallBackDeselect;
        _tapGesture.StateUpdated += TapGestureCallBack;
        SelectedStuff = null;
        DisableControlStuffButtons();
    }

    private void EnableControlStuffButtons()
    {
        ChangeMaterialBtn.interactable = true;
        DeleteStuffBtn.interactable = true;
    }

    private void DisableControlStuffButtons()
    {
        ChangeMaterialBtn.interactable = false;
        DeleteStuffBtn.interactable = false;
    }


    private void ResetScene()
    {
        DeselectSelectedStuff();
        arSession.Reset();
        // arStuffManager.Reset();
    }

    public void DisplayPlane()
    {
        try
        {
            arPlaneMaterial.color = new Color(1, 1, 1, 1);
            arPlaneMaterial.SetColor("_TexTintColor", new Color(1, 1, 1, 1));
        }
        catch
        {
            Debug.Log("Failed finding the following properties");
        }
    }

    public void HidePlane()
    {
        try
        {
            arPlaneMaterial.color = new Color(1, 1, 1, 0);
            arPlaneMaterial.SetColor("_TexTintColor", new Color(1, 1, 1, 0));
        }
        catch
        {
            Debug.Log("Failed finding the following properties");
        }
    }

    private GestureTouch FirstTouch(ICollection<GestureTouch> touches)
    {
        foreach (GestureTouch t in touches)
        {
            return t;
        }

        return new GestureTouch();
    }

    private void CreatTapGesture()
    {
        _tapGesture = new TapGestureRecognizer();
        _tapGesture.StateUpdated += TapGestureCallBack;
        FingersScript.Instance.AddGesture(_tapGesture);
    }

    private void TapGestureCallBack(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            GestureTouch t = FirstTouch(gesture.CurrentTrackedTouches);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(t.X, t.Y)), 100f);
            ArStuff hittedStuff = null;

            foreach (RaycastHit r in hits)
            {
                hittedStuff = r.collider.GetComponent<ArStuff>();
                if (hittedStuff == null)
                    continue;

                SelectStuff(hittedStuff);
                _tapGesture.StateUpdated -= TapGestureCallBack;
                _tapGesture.StateUpdated += TapGestureCallBackDeselect;
                return;
            }

            if (curStuff == null)
                return;

            List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

            if (arRaycastManager.Raycast(new Vector2(t.X, t.Y), arRaycastHits, TrackableType.Planes))
            {
                ArStuff script = arStuffManager.Spawn(curStuff, arRaycastHits[0].pose.position);
                curStuff = null;
            }
        }
    }

    private void TapGestureCallBackDeselect(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (MaterialsManager.Instance.curState == MaterialsManager.State.None)
                DeselectSelectedStuff();
        }
    }
}
