namespace Forum.Services
{
    public class RandomServices
    {
        private Random random = new Random();
        public int Integer => random.Next();
    }
}
