using System.Collections.Generic;
using DigitalRubyShared;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArStuff : MonoBehaviour
{
    // CORE VARIABLES
    public MeshRenderer clothMeshRenderer;
    public MeshRenderer cloth2MeshRenderer;
    public MeshRenderer woodMeshRenderer;
    public MeshRenderer wood2MeshRenderer;
    public MeshRenderer metalMeshRenderer;
    public MeshRenderer plasticMeshRenderer;
    public MeshRenderer colorMeshRenderer;
    public MeshRenderer otherMeshRenderer;
    public MeshRenderer other2MeshRenderer;
    public MeshRenderer other3MeshRenderer;

    // BACKUPS
    private Material _clothMain;
    private Material _cloth2Main;
    private Material _woodMain;
    private Material _wood2Main;
    private Material _metalMain;
    private Material _plasticMain;
    private Material _colorMain;
    private Material _otherMain;
    private Material _other2Main;
    private Material _other3Main;

    // Data
    [SerializeField]
    public MaterialFields Fields;

    [SerializeField] public List<cakeslice.Outline> outlines;

    // UTILITIES
    protected RotateGestureRecognizer RotateGesture;
    protected PanGestureRecognizer PanGesture;

    private bool _isSelected = false;
    private bool _isMoving = false;
    private bool _startedWithThis;

    // Start is called before the first frame update
    protected void Start()
    {
        RotateGesture = new RotateGestureRecognizer();
        PanGesture = new PanGestureRecognizer();
        // ChangeAllMaterials(ArSceneManager.Instance.defaultMaterial);
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    public void Select()
    {
        CheckSelection();
    }

    public void Deselect()
    {
        CheckDeselection();
    }

    protected void CheckSelection()
    {
        if (!_isSelected)
        {
            RotateGesture.StateUpdated += RotateStuffCallBack;
            if (!FingersScript.Instance.Gestures.Contains(RotateGesture))
                FingersScript.Instance.AddGesture(RotateGesture);

            PanGesture.StateUpdated += PanMoveStuffCallBack;
            if (!FingersScript.Instance.Gestures.Contains(PanGesture))
                FingersScript.Instance.AddGesture(PanGesture);

            _isSelected = true;
            ToggleOutlines();
        }
    }


    protected void CheckDeselection()
    {
        if (_isSelected)
        {
            RotateGesture.StateUpdated -= RotateStuffCallBack;
            if (FingersScript.Instance.Gestures.Contains(RotateGesture))
                FingersScript.Instance.RemoveGesture(RotateGesture);

            PanGesture.StateUpdated -= PanMoveStuffCallBack;
            if (FingersScript.Instance.Gestures.Contains(PanGesture))
                FingersScript.Instance.RemoveGesture(PanGesture);

            _isSelected = false;
            ToggleOutlines();
        }
    }

    protected void RotateStuffCallBack(GestureRecognizer gesture)
    {
        if (MaterialsManager.Instance.curState != MaterialsManager.State.None)
            return;

        if (gesture.State == GestureRecognizerState.Began)
        {
            MakeMovingMaterial();
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            this.gameObject.transform.Rotate(0f, -RotateGesture.RotationRadiansDelta * Mathf.Rad2Deg * 2, 0f);
        }
        else if (gesture.State == GestureRecognizerState.Ended || gesture.State == GestureRecognizerState.Failed)
        {
            RestoreBackUpMaterials();
        }
    }

    protected void PanMoveStuffCallBack(GestureRecognizer gesture)
    {
        if (MaterialsManager.Instance.curState != MaterialsManager.State.None)
            return;
        GestureTouch t = FirstTouch(gesture.CurrentTrackedTouches);

        if (gesture.State == GestureRecognizerState.Began)
        {
            var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(t.X, t.Y)), 100f);

            foreach (RaycastHit r in hits)
            {
                var hittedStuff = r.collider.GetComponent<ArStuff>();
                if (hittedStuff == null)
                    continue;

                _startedWithThis = hittedStuff == this;
                if (_startedWithThis)
                {
                    _isMoving = true;
                    //ArSceneManager.Instance.DisplayPlane();
                    MakeMovingMaterial();
                    ToggleOutlines();
                    break;
                }
            }
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            if (_startedWithThis == false)
                return;

            List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

            if (ArSceneManager.Instance.arRaycastManager.Raycast(new Vector2(t.X, t.Y), arRaycastHits,
                TrackableType.Planes))
            {
                this.gameObject.transform.position = arRaycastHits[0].pose.position;
            }
        }
        else if (gesture.State == GestureRecognizerState.Ended || gesture.State == GestureRecognizerState.Failed)
        {
            if (_startedWithThis)
            {
                _isMoving = false;
                //ArSceneManager.Instance.HidePlane();
                RestoreBackUpMaterials();
                ToggleOutlines();
            }

            _startedWithThis = false;
        }
    }

    public void ChangeMaterial(Material ongoingMaterial, Layer ongoingLayer)
    {
        switch (ongoingLayer)
        {
            case Layer.Cloth:
                {
                    if (clothMeshRenderer == null)
                        break;

                    clothMeshRenderer.material = ongoingMaterial;
                    if (clothMeshRenderer.gameObject != null)
                    {
                        int count = clothMeshRenderer.gameObject.transform.childCount;
                        for (int i = 0; i < count; i++)
                        {
                            clothMeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                                ongoingMaterial;
                        }
                    }

                    break;
                }

            case Layer.Cloth2:
                {
                    if (cloth2MeshRenderer == null)
                        break;

                    cloth2MeshRenderer.material = ongoingMaterial;
                    if (cloth2MeshRenderer.gameObject != null)
                    {
                        int count = cloth2MeshRenderer.gameObject.transform.childCount;
                        for (int i = 0; i < count; i++)
                        {
                            cloth2MeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                                ongoingMaterial;
                        }
                    }

                    break;
                }
            case Layer.Wood:
                {
                    if (woodMeshRenderer == null)
                        break;

                    woodMeshRenderer.material = ongoingMaterial;
                    if (woodMeshRenderer.gameObject != null)
                    {
                        int count = woodMeshRenderer.gameObject.transform.childCount;
                        for (int i = 0; i < count; i++)
                        {
                            woodMeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                                ongoingMaterial;
                        }
                    }

                    break;
                }

            case Layer.Wood2:
                {
                    if (wood2MeshRenderer == null)
                        break;

                    wood2MeshRenderer.material = ongoingMaterial;
                    int count = wood2MeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        wood2MeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }

            case Layer.Metal:
                {
                    if (metalMeshRenderer == null)
                        break;

                    metalMeshRenderer.material = ongoingMaterial;
                    int count = metalMeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        metalMeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }
            case Layer.Plastic:
                {
                    if (plasticMeshRenderer == null)
                        break;

                    plasticMeshRenderer.material = ongoingMaterial;
                    int count = plasticMeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        plasticMeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }
            case Layer.Color:
                {
                    if (colorMeshRenderer == null)
                        break;

                    colorMeshRenderer.material = ongoingMaterial;
                    int count = colorMeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        colorMeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }

            case Layer.Other:
                {
                    if (otherMeshRenderer == null)
                        break;

                    otherMeshRenderer.material = ongoingMaterial;
                    int count = otherMeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        otherMeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }

            case Layer.Other2:
                {
                    if (other2MeshRenderer == null)
                        break;

                    other2MeshRenderer.material = ongoingMaterial;
                    int count = other2MeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        other2MeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }

            case Layer.Other3:
                {
                    if (other3MeshRenderer == null)
                        break;

                    other3MeshRenderer.material = ongoingMaterial;
                    int count = other3MeshRenderer.gameObject.transform.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        other3MeshRenderer.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                            ongoingMaterial;
                    }

                    break;
                }
        }
    }


    private void MakeMovingMaterial()
    {
        BackUpMaterials();
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Cloth);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Cloth2);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Wood);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Wood2);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Metal);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Plastic);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Color);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Other);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Other2);
        ChangeMaterial(ArSceneManager.Instance.onMovingMaterial, Layer.Other3);
    }

    protected void BackUpMaterials()
    {
        if (clothMeshRenderer != null)
            _clothMain = new Material(clothMeshRenderer.material);

        if (cloth2MeshRenderer != null)
            _cloth2Main = new Material(cloth2MeshRenderer.material);

        if (woodMeshRenderer != null)
            _woodMain = new Material(woodMeshRenderer.material);

        if (wood2MeshRenderer != null)
            _wood2Main = new Material(wood2MeshRenderer.material);

        if (metalMeshRenderer != null)
            _metalMain = new Material(metalMeshRenderer.material);

        if (plasticMeshRenderer != null)
            _plasticMain = new Material(plasticMeshRenderer.material);

        if (colorMeshRenderer != null)
            _colorMain = new Material(colorMeshRenderer.material);

        if (otherMeshRenderer != null)
            _otherMain = new Material(otherMeshRenderer.material);

        if (other2MeshRenderer != null)
            _other2Main = new Material(other2MeshRenderer.material);

        if (other3MeshRenderer != null)
            _other3Main = new Material(other3MeshRenderer.material);
    }

    private void RestoreBackUpMaterials()
    {
        if (CheckLayer(Layer.Cloth))
            clothMeshRenderer.material = _clothMain;

        if (CheckLayer(Layer.Cloth2))
            cloth2MeshRenderer.material = _cloth2Main;

        if (CheckLayer(Layer.Wood))
            woodMeshRenderer.material = _woodMain;

        if (CheckLayer(Layer.Wood2))
            wood2MeshRenderer.material = _wood2Main;

        if (CheckLayer(Layer.Metal))
            metalMeshRenderer.material = _metalMain;

        if (CheckLayer(Layer.Plastic))
            plasticMeshRenderer.material = _plasticMain;

        if (CheckLayer(Layer.Color))
            colorMeshRenderer.material = _colorMain;

        if (CheckLayer(Layer.Other))
            otherMeshRenderer.material = _otherMain;

        if (CheckLayer(Layer.Other2))
            other2MeshRenderer.material = _other2Main;

        if (CheckLayer(Layer.Other3))
            other3MeshRenderer.material = _other3Main;
    }

    private void ChangeAllMaterials(Material material)
    {
        if (material == null)
            return;

        ChangeMaterial(material, Layer.Cloth);
        ChangeMaterial(material, Layer.Cloth2);
        ChangeMaterial(material, Layer.Wood);
        ChangeMaterial(material, Layer.Wood2);
        ChangeMaterial(material, Layer.Metal);
        ChangeMaterial(material, Layer.Plastic);
        ChangeMaterial(material, Layer.Color);
        ChangeMaterial(material, Layer.Other);
        ChangeMaterial(material, Layer.Other2);
        ChangeMaterial(material, Layer.Other3);
    }


    void ToggleOutlines()
    {
        foreach (var outline in outlines)
        {
            outline.eraseRenderer = !outline.eraseRenderer;
        }
    }

    private GestureTouch FirstTouch(ICollection<GestureTouch> touches)
    {
        foreach (GestureTouch t in touches)
        {
            return t;
        }

        return default;
    }

    public void Remove()
    {
        FingersScript.Instance.RemoveGesture(RotateGesture);
        FingersScript.Instance.RemoveGesture(PanGesture);
    }

    public bool CheckLayer(Layer layer)
    {
        bool res = false;

        switch (layer)
        {
            case Layer.Cloth:
                {
                    if (clothMeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Cloth2:
                {
                    if (cloth2MeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Wood:
                {

                    if (woodMeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Wood2:
                {
                    if (wood2MeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Metal:
                {
                    if (metalMeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Plastic:
                {
                    if (plasticMeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Color:
                {
                    if (colorMeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Other:
                {
                    if (otherMeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Other2:
                {
                    if (other2MeshRenderer != null)
                        res = true;
                    break;
                }
            case Layer.Other3:
                {
                    if (other3MeshRenderer != null)
                        res = true;
                    break;
                }
        }

        return res;
    }

    public enum Layer
    {
        Cloth = 0,
        Cloth2 = 1,
        Wood = 2,
        Wood2 = 3,
        Metal = 4,
        Plastic = 5,
        Color = 6,
        Other = 7,
        Other2 = 8,
        Other3 = 9
    }
}
