using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yakanashe.Wiper
{
    [CreateAssetMenu(fileName = "TransitionCollection", menuName = "Wiper/Transition Collection")]
    public class TransitionCollection : ScriptableObject
    {
        public List<TransitionData> transitions = new();
    }

    [Serializable]
    public class TransitionData
    {
        public string name;
        public Shader shader;
    }
}