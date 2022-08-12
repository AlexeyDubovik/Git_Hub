namespace Forum.Services
{
    public class SessionAuthService : IAuthService
    {
        private readonly DAL.Context.IntroContext _context;
        public DAL.Entities.User? User { get; set; }
        public SessionAuthService(DAL.Context.IntroContext context)
        {
            _context = context;
        }
        public void Set(string id)
        {
            if (id != null)
                User = _context.Users!.Find(Guid.Parse(id!));
        }
    }
}
