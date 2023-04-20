using Game.Models;
using UnityEngine;

namespace UI.Views
{
    public class UIRewardedButton : UIView
    {

        public void Initialize(GameInfo gameInfo)
        {
            gameInfo.OnStartGame += Show;
            gameInfo.OnLoseGame += Hide;
            gameInfo.OnWinGame += Hide;
        }
        
        
    }
}