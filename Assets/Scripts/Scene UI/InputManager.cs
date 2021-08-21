using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Back();
    }

    private void Back()
    {
        if (MaterialsManager.Instance.curState != MaterialsManager.State.None)
        {
            MaterialsManager.Instance.Back();
        }
    }
}
