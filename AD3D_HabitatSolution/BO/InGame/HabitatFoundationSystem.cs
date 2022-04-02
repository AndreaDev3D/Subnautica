using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_HabitatSolutionMod.BO.InGame
{
    public class HabitatFoundationSystem : MonoBehaviour
    {
        public float speed = 1.0f;
        private Vector3 target;

        public void Start()
        {
            target = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
            StartCoroutine(MoveUp());
        }

        IEnumerator MoveUp()
        {
            //float step = speed * Time.deltaTime; // calculate distance to move
            //transform.position = Vector3.MoveTowards(transform.position, target, step);
            //yield return null;
            var lerpV = 0.0f;
            while (lerpV < 1.0f)
            {
                transform.position = Vector3.Lerp(transform.position, target, lerpV);
                lerpV += 0.025f;
                yield return new WaitForSeconds(0.1f);
                //    target.position *= -1.0f;
                //    // Check if the position of the cube and sphere are approximately equal.
                //    if (Vector3.Distance(transform.position, target) < 0.001f)
                //    {
                //        // Swap the position of the cylinder.
                //        target.position *= -1.0f;
                //    }
                //    yield return new WaitForSeconds(0.5);
                //    Debug.Log("Sending");
            }
        }
    }
}
