using Features.Game;

namespace Features.Player
{
    public class KillablePlayer : Killable
    {
        public void CrashAndBurn()
        {
            Die();
        }
    }
}
