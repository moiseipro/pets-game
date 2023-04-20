using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Views
{
    public class UIPetLevel : MonoBehaviour
    {
        [SerializeField] private UIPetStar _uiPetStarPrefab;
        private List<UIPetStar> _uiPetStars = new List<UIPetStar>();

        private void DeleteOldPetStar()
        {
            if (_uiPetStars.Count != 0)
            {
                for(int i = _uiPetStars.Count - 1; i >= 0; i--)
                {
                    _uiPetStars[i].Death();
                }
                _uiPetStars.Clear();
            }
        }
        
        public void UpdatePetStar(int level)
        {
            DeleteOldPetStar();
            for (int i = 0; i < level; i++)
            {
                _uiPetStars.Add(Instantiate(_uiPetStarPrefab, transform));
                _uiPetStars[i].LoadLevel();
            }
        }
        
        public void UpPetStar(int level)
        {
            DeleteOldPetStar();
            for (int i = 0; i < level; i++)
            {
                _uiPetStars.Add(Instantiate(_uiPetStarPrefab, transform));
                _uiPetStars[i].LoadLevel(i);
            }
        }
    }
}