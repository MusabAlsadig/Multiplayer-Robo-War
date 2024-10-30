using UnityEngine;
using static UnityEngine.Object;

namespace HelpingMethods
{
    public static class Editor
    {

        public static void MakeDirty(GameObject _object)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_object);
#endif
        }

        public static void DestroyComponentOnPrefab<T>(GameObject _object) where T : UnityEngine.Object
        {
            GameObject prefab = _object.transform.root.gameObject;
            MakeDirty(prefab);
            DestroyImmediate(_object.GetComponent<T>(), true);
            ClearDirty(prefab);
        }

        public static void DestroyObjectOnPrefab(GameObject _object)
        {
            GameObject prefab = _object.transform.root.gameObject;
            MakeDirty(prefab);
            DestroyImmediate(_object, true);
            ClearDirty(prefab);
        }

        public static void ClearDirty(GameObject _object)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearDirty(_object);
#endif
        }
    }

}
