using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CopyMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();

        var newMesh = MakeCopy(meshFilter.sharedMesh);

        meshFilter.sharedMesh = newMesh;
    }
    
    private Mesh MakeCopy(Mesh src)
    {
        //null checks
        if (src == null)
        {
            return null;
        }

        Mesh dst = new Mesh();
        Copy(src, dst);
        return dst;
    }

    private void Copy(Mesh src, Mesh dst)
    {
        dst.Clear();
        dst.vertices = src.vertices;

        List<Vector4> uvs = new List<Vector4>();

        src.GetUVs(0, uvs); dst.SetUVs(0, uvs);
        src.GetUVs(1, uvs); dst.SetUVs(1, uvs);
        src.GetUVs(2, uvs); dst.SetUVs(2, uvs);
        src.GetUVs(3, uvs); dst.SetUVs(3, uvs);

        dst.normals = src.normals;
        dst.tangents = src.tangents;
        dst.boneWeights = src.boneWeights;
        dst.colors = src.colors;
        dst.colors32 = src.colors32;
        dst.bindposes = src.bindposes;

        dst.subMeshCount = src.subMeshCount;
        dst.indexFormat = src.indexFormat;

        for(int i = 0; i < src.subMeshCount; i++)
            dst.SetIndices(src.GetIndices(i), src.GetTopology(i), i);

        dst.name = src.name + "_Copy";
    }
    
}
