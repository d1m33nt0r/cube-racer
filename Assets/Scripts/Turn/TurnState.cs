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
    }
}