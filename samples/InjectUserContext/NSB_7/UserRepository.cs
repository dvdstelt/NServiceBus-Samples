namespace InjectUserContext
{
    public class UserRepository
    {
        public bool CanDeleteRows(string userId)
        {
            if (userId == "42")
                return true;

            return false;
        }
    }
}
