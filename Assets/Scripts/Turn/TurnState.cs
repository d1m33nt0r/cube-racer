namespace DefaultNamespace
{
    public static class TurnState
    {
        public enum State
        {
            Forward,
            Right,
            Back,
            Left
        }

        public static State state;
        public static State prevState;
        
        public static void SetState(State _state)
        {
            prevState = state;
            state = _state;
        }
    }
}