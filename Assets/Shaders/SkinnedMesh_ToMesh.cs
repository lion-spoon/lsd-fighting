using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMesh_ToMesh : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMesh;
    public VisualEffect VFXgraph;
    public float refreshrate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(updateVFXgraph());
    }

    IEnumerator updateVFXgraph()
    {
        while (gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            SkinnedMesh.BakeMesh(m);

            Vector3[] vertices = m.vertices;
            Mesh m2 = new Mesh();
            m2.vertices = vertices;

            VFXgraph.SetMesh("Mesh", m2);
            

            yield return new WaitForSeconds(refreshrate);
        }
    }

}
