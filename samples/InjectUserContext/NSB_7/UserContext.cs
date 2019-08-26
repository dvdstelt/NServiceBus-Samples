namespace InjectUserContext
{
    public class UserContext
    {
        public string UserId { get; set; }

        // Some random right this user has
        public bool CanDeleteRows { get; set; }
    }
}
