using Pets.Models;
using UnityEngine;

namespace Pets.Data
{
    [CreateAssetMenu(fileName = "New pet model", menuName = "PetModel", order = 0)]
    public class PetModelScriptable : ScriptableObject
    {
        public PetModel PetModel;
    }
}