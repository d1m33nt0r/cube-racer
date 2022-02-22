namespace DefaultNamespace
{
    public class TurnState
    {
        public enum State
        {
            Forward,
            Right,
            Back,
            Left
        }

        public State state;
        public State prevState;
        
        public void SetState(State _state)
        {
            prevState = state;
            state = _state;
        }
    }
}