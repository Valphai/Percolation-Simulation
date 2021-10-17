namespace Grid
{
    public enum Direction 
    {
        N, NE, E, SE, S, SW, W, NW
    }
    
    public static class DirectionExtensions 
    {
        public static Direction Opposite (this Direction direction) 
        {
            return (int)direction < 4 ? (direction + 4) : (direction - 4);
        }
    
        public static Direction Previous (this Direction direction) 
        {
            return direction == Direction.N ? Direction.NW : (direction - 1);
        }
    
        public static Direction Next (this Direction direction) 
        {
            return direction == Direction.NW ? Direction.N : (direction + 1);
        }
    }
}