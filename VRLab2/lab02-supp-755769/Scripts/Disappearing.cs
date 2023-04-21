using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Disappearing : MonoBehaviour
{
    public float maxDistance = 5f;
    public float minDistance = 1.2f;

    public Transform player;
    private SkinnedMeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    void Update()
    {
        // Calculate transparency depending on distance
        float transparency = Mathf.Clamp(((meshRenderer.transform.position - player.position).magnitude - minDistance) / maxDistance , 0, 1);
        // You have to select the property name, not the display name
        meshRenderer.material.SetFloat("_Transparency", transparency);
    }
}

