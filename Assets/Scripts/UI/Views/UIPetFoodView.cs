using Ability.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class UIPetFoodView : MonoBehaviour
    {
        [SerializeField] private RectTransform _foodGroup;

        public void UpdateStatValue(FoodType[] foodTypes)
        {
            Image[] images = _foodGroup.GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                Destroy(images[i].gameObject);
            }

            foreach (var foodType in foodTypes)
            {
                Instantiate(Resources.Load("UIFood/" + foodType), _foodGroup);
            }
            
        }
    }
}