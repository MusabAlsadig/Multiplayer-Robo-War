using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Object;

namespace HelpingMethods
{
    public static class ObjectMethods
    {
        public static T[] GetComponentsInDeepChildren<T>(Transform transformToSearch)
        {
            List<T> components = new List<T>();
            int i = 0;
            T foundComponent;

            SearchNextLevel(transformToSearch);

            return components.ToArray();

            void SearchNextLevel(Transform childToSearch)
            {
                foreach (Transform _transform in childToSearch)
                {
                    i++;

                    if (i > 1000)
                    {
                        Debug.LogError("this search took 1000+ trys, could be an infinite loop");
                        return;
                    }
                    if (_transform.TryGetComponent(out foundComponent))
                        components.Add(foundComponent);

                    SearchNextLevel(_transform);

                }
            }
        }


        /// <summary>
        /// Stop once reach <paramref name="levels"/> of deep searching
        /// <br/>
        /// <b>Example : </b> levels = 2 means search until grandchildren
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transformToSearch"></param>
        /// <param name="levels"></param>
        /// <returns></returns>
        public static T[] GetComponentsOnDeepChildren<T>(Transform transformToSearch, int levels)
        {
            List<T> components = new List<T>();
            int i = 0;
            T foundComponent;

            SearchNextLevel(transformToSearch);

            return components.ToArray();

            void SearchNextLevel(Transform childToSearch)
            {
                if (levels <= 0)
                    return;
                else
                    levels--;

                foreach (Transform _transform in childToSearch)
                {
                    i++;

                    if (i > 1000)
                    {
                        Debug.LogError("this search took 1000+ trys, could be an infinite loop");
                        return;
                    }
                    if (_transform.TryGetComponent(out foundComponent))
                        components.Add(foundComponent);

                    SearchNextLevel(_transform);
                }
            }
        }

        public static void ChangeLocalScaleAxis(Transform transform, Axis axis, float value)
        {
            Vector3 localScale = transform.localScale;

            switch (axis)
            {
                case Axis.x:
                    localScale.x = value;
                    break;
                case Axis.y:
                    localScale.y = value;
                    break;
                case Axis.z:
                    localScale.z = value;
                    break;
            }

            transform.localScale = localScale;
        }

        /// <summary>
        /// remove all children from <paramref name="transform"/>
        /// </summary>
        /// <param name="transform"></param>
        public static void ClearTransform(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// remove all children from <paramref name="transform"/> but the ones with index less than <paramref name="startFrom"/>
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="startFrom">first index to start deleting</param>
        /// <param name="leave">how many to not destroy (from the end) </param>
        public static void ClearTransform(Transform transform, int startFrom, int leave = 0)
        {
            for (int i = startFrom; i < transform.childCount - leave; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }



        /// <summary>
        /// Remove all children from <paramref name="transform"/> 
        /// then creat <paramref name="amount"/> of <paramref name="child_Prefab"/> copys
        /// then add <paramref name="component"/> if not null
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="child_Prefab"></param>
        /// <param name="amount"></param>
        public static void ResetTransform(Transform transform, GameObject child_Prefab, int amount, Component component = null)
        {
            ClearTransform(transform);

            for (int i = 0; i < amount; i++)
            {
                var child = Object.Instantiate(child_Prefab, transform);
                if (component)
                    child.AddComponent(component.GetType());
            }
        }
    }

}
