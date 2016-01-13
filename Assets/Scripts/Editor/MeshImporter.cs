using UnityEngine;
using UnityEditor;
 
public class MeshPostprocessor :  AssetPostprocessor {
 
    // Use this for initialization Import Settings global "Scale Factor" = 1:1 and not 0.01
    void OnPreprocessModel () {
        (assetImporter as ModelImporter).globalScale = 0.025f;
    }
}