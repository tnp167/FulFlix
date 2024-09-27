using HotChocolate;
using backend.Interfaces;

namespace backend.GraphQL{

public class UserQuery
{
    public string HelloWorld()
    {
        return "Hello, from GraphQL!";
    }
}

}
