namespace Bosses.Chest
{
    public class StateMachine
    {
        public BossState State;

        public void Set(BossState newState, bool forceReset = false)
        {
            if (State != newState || forceReset)
            {
                State?.Exit();
                State = newState;
                State.Initialise(this);
                State.Enter();
            }
        }
    }
}
