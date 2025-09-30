using GYMPT.Domain;
using GYMPT.Models;

namespace GYMPT.Factories
{
    public class ClientCreator : UserCreator
    {
        public override User CreateUser()
        {
            return new Client();
        }
    }
}