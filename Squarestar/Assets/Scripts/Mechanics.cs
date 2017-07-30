using UnityEngine;
using System.Collections;

namespace Mechanics
{
    [System.Serializable]
    public class Boost // : ScriptableObject
    {
        [SerializeField]
        private float amount;
        [SerializeField]
        private float decayRate;
        // Constructors

        public Boost() { } // Default...

        public Boost(float _amount, float _decayRate)
        {
            amount    = _amount;
            decayRate = _decayRate;
        }

        // Setters / Getters
        #region properties

        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public float DecayRate
        {
            get { return decayRate; }
            set { decayRate = value; }
        }

        #endregion

    }
}
