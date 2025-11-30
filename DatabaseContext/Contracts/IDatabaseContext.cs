using Domain;

namespace Database.Contracts
{
    public interface IDatabaseContext
    {
        List<T> GetData<T>(ParameterisedQuery query);
        int Execute(ParameterisedQuery query);
        object ExecuteScalar(ParameterisedQuery query);
    }
}
