using System;

namespace InjectUserContext
{
    public class MyDependency
    {
        private readonly UserContext userContext;

        public MyDependency(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public void DeleteSomething(string someValue)
        {
            if (!userContext.CanDeleteRows)
            {
                throw new SystemException("Sorry, cannot execute, because I cannot delete rows!");
            }

            // Delete rows
            Console.WriteLine("Row deleted");
        }
    }
}