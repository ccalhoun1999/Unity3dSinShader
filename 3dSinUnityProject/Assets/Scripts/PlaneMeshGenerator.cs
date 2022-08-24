using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlaneMeshGenerator : MonoBehaviour
{
    [SerializeField]
    private int serialSize;

    private int size{
        get { return _size; }
        set
        {
            _size = value;
            if(_size > 254)
            {
                size = 254;
            }
            mesh.Clear();
            GenerateMesh();
        }
    } private int _size;

    private Vector3[] vertices;
    private Mesh mesh;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        size = 100;

        GenerateMesh();
    }

    private void Update()
    {
        if (size != serialSize)
        {
            size = serialSize;
            mesh.bounds.size.Set(1000f, 1000f, 1000f);
        }
    }

    private void GenerateMesh()
    {
        vertices = new Vector3[(size + 1) * (size + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0, z = - size / 2; z < size / 2 + 1; z++)
        {
            for (int x = - size / 2; x < size / 2 + 1; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2(x / size, z / size);
            }
        }

        int[] triangles = new int[size * size * 6];
		for (int ti = 0, vi = 0, z = 0; z < size; z++, vi++)
        {
            for (int x = 0; x < size; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + size + 1;
                triangles[ti + 5] = vi + size + 2;
            }
		}
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }
}
