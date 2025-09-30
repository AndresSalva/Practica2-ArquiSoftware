using GYMPT.Domain;
using GYMPT.Models;

namespace GYMPT.Factories
{

    public class InstructorCreator : UserCreator
    {
        public override User CreateUser()
        {
            return new Instructor();
        }
    }
}