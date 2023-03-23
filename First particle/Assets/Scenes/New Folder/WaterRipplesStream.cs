using System;
using UnityEngine;

[ExecuteInEditMode]
public class WaterRipplesStream : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float radius;

    private void Update()
    {
        if (material == null) return;
        Vector3 p = transform.position;
        material.SetVector("_RipplesCenter",new Vector4(p.x,p.y,p.z, radius));
    }
}
