using TMPro;
using UnityEngine;

namespace UI
{
    public class GameResultUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _gameOverText;

        private void Awake()
        {
            DeActivate();
        }

        public void SetResult(string result)
        {
            _gameOverText.SetText(result);
        }
        
        public void Activate()
        {
            _gameOverText.enabled = true;
        }
        
        public void DeActivate()
        {
            _gameOverText.enabled = false;
        }
    }
}