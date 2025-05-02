using UnityEngine;

namespace GameCore
{
    public class GameTime
    {
        public float DeltaTime => Time.deltaTime * TimeScale;
        
        public float TimeScale = 1f;
    }
}