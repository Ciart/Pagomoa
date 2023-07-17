using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour
{
    public Transform player;
    public float viewRange = 2;
    public float fadeOutSpeed = 2;
    public float restoreSpeed = 0.2f;

    Mesh mesh;
    MeshFilter meshFilter;

    Vector3[] verts;
    Color[] cols;

    void Awake()
    {

        GetComponent<Renderer>().enabled = true;
        GetComponent<Renderer>().sortingOrder = 20;  //Mesh 쪽은 3D 쪽이라 Sorting layer 사용이 원활하지 않아서 코드로 직접 건듦.
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        verts = mesh.vertices;

        cols = new Color[verts.Length];
        for (int i = 0; i < verts.Length; i++)
            cols[i] = new Color(0, 0, 0, 1);
        mesh.colors = cols;


    }

    void Update()
    {

        for (int j = 0; j < verts.Length; j++)
        {
            float distance = Vector3.Distance(player.position, transform.TransformPoint(verts[j]));
            if ((distance <= viewRange))
                cols[j].a = Mathf.Clamp01(cols[j].a - (viewRange - distance) * Time.deltaTime * fadeOutSpeed); // fading
        }
        mesh.colors = cols;
        meshFilter.mesh = mesh;
    }
}

