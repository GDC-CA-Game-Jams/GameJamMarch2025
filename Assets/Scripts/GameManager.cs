using Util.Services;

namespace Gameplay
{
    public class GameManager : IService
    {
        private int score = 0;

        public int Score
        {
            get
            {
                return score;
            }
        }
        
        public void SetScore(int newScore)
        {
            score = newScore;
        }
        
        public void Dispose()
        {
            
        }
    }
}