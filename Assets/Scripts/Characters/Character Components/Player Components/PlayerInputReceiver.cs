using UnityEngine;

namespace CGames
{
    public class PlayerInputReceiver : MonoBehaviour
    {
        private CharacterData characterData;

        public void Initialize(CharacterData characterData)
        {
            this.characterData = characterData;
        }

        private void Update() => characterData.SetNewInput(PlayerInputHandler.PlayerInput);
    }
}