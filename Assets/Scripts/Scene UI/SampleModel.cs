using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleModel : MonoBehaviour
{
    [SerializeField]
    public MeshRenderer Renderer;

    [SerializeField]
    private Vector3 _speed;

    private void Rotate(Vector3 speed)
    {
        transform.Rotate(speed * Time.deltaTime, Space.Self);
    }

    void Update()
    {
        Rotate(_speed);
    }
}
