using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_HabitatSolutionMod.BO.InGame
{
    public class StoragebaleItemSystem : MonoBehaviour, IProtoEventListener
    {
        void Start()
        {

        }

        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            //throw new NotImplementedException();
        }

        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            //throw new NotImplementedException();
        }
    }
}
