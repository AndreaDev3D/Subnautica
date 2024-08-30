using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_Common.Utils
{
    public static class GameObjectFinder
    {
        public static GameObject FindByName(this GameObject go, string name, bool includeInactive = true)
        {
            Transform[] ts = go.GetComponentsInChildren<Transform>(includeInactive);
            foreach (var t in ts) 
                if (t.gameObject.name == name) return t.gameObject;

            return null;
        }

        // Extension method to find a child by name and return a component of the specified type
        public static T FindComponentByName<T>(this GameObject parent, string childName, bool includeInactive = true) where T : Component
        {
            // Get all children and nested children of the GameObject
            Transform[] allChildren = parent.GetComponentsInChildren<Transform>(includeInactive); // true includes inactive objects

            // Iterate through each child
            foreach (var child in allChildren)
            {
                if (child.name == childName)
                {
                    // Return the component of type T if found
                    return child.GetComponent<T>();
                }
            }

            // If no child with the specified name is found, return null
            return null;
        }
    }
}
