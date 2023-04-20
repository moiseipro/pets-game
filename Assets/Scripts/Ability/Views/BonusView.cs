using DG.Tweening;
using Effects.Views;
using Game.Models;
using Level.Views;
using UnityEngine;

namespace Ability.Views
{
    public class BonusView : MonoBehaviour
    {
        private ComboCheckerModel _comboCheckerModel;
        private ParticleEffectPool _deathEffectPool;
        private PetFlask _petFlask;
        
        private Sequence _sequence;
        
        public void Initialize(ComboCheckerModel comboCheckerModel, ParticleEffectPool deathEffectPool, PetFlask petFlask)
        {
            _comboCheckerModel = comboCheckerModel;
            _deathEffectPool = deathEffectPool;
            _petFlask = petFlask;
        }
    }
}