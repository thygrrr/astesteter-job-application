using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Tiger.Util
{
    public static class GameObjectExtensions
    {    
        /// <summary>Instantiates a copy of this GameObject</summary>
        public static GameObject Instantiate(this GameObject go)
        {
            return Object.Instantiate(go, go.transform.parent);
        }

        /// <summary>Instantiates a copy of this GameObject in a given parent</summary>
        public static GameObject Instantiate(this GameObject go, Transform parent)
        {
            return Object.Instantiate(go, parent);
        }
        
        /// <summary>Instantiates amount copies of this GameObject in a given parent, deactivates them.</summary>
        public static GameObject[] InstantiatePool(this GameObject go, Transform parent, int amount, HideFlags hide_flags = HideFlags.HideAndDontSave)
        {
            var result = new GameObject[amount];
            for (var i = 0; i < amount; i++)
            {
                result[i] = Object.Instantiate(go, parent);
                result[i].SetActive(false);
                result[i].hideFlags = hide_flags;
            }
            return result ;
        }
        
        
        /// <summary>
        /// Copies a component and attaches it to this gameobject
        /// </summary>
        /// <param name="destination">the GameObject</param>
        /// <param name="original">the Component</param>
        /// <typeparam name="T">type of the Component</typeparam>
        /// <returns>The new instance of the Component</returns>
        public static T CopyComponent<T>(this GameObject destination, T original) where T : Component
        {
            var type = original.GetType();

            var dst = destination.AddComponent(type) as T;

            /* This causes problems - copies some internal fields, clobbering the components.
            var fields = GetAllFields(type);
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            */
            
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanRead || prop.Name == "name") continue;
                if (prop.GetCustomAttribute<ObsoleteAttribute>() != null) continue;
                
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            
            return dst;
        }

        private static IEnumerable<FieldInfo> GetAllFields(System.Type t)
        {
            if (t == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            var flags = BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.DeclaredOnly;
            
            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }
    }
}
