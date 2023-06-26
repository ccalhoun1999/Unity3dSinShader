using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ColliderAdderScript : MonoBehaviour
{
    private void OnValidate()
    {
        MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();

        foreach(MeshRenderer mesh in meshes)
        {
            if (mesh.gameObject.layer == LayerMask.NameToLayer("GrappleGeometry")
                && mesh.gameObject.GetComponent<MeshCollider>() == null)
            {
                mesh.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
}
