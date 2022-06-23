#if UNITY_EDITOR
/*
using _Script;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactible)), CanEditMultipleObjects]
public class InteractibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactible inter = (Interactible) target;
        
        inter.type = (InteractType)EditorGUILayout.EnumPopup("Type: ",inter.type);

        switch (inter.type)
        {
            case InteractType.Spawner:

                inter.throwablePrefab = (GameObject)EditorGUILayout.ObjectField("Prefab Throwable: ",inter.throwablePrefab, typeof(GameObject), false);
                
                break;
            case InteractType.Throwable:
                inter.dataObject = (ObjectData)EditorGUILayout.ObjectField("Data Objet: ", inter.dataObject, typeof(ObjectData), false);
                
                break;
            case InteractType.Player:
                break;
            case InteractType.Comptoir:
                break;
            case InteractType.PassePlat:
                break;
            case InteractType.Etabli:
                inter.throwablePrefab = (GameObject)EditorGUILayout.ObjectField(inter.throwablePrefab, typeof(GameObject), false);
                
                break;
        }
    }
}
*/
#endif