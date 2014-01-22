namespace ServiceBusRelay.Console
{
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Name = "ICustomerContract", Namespace = "http://samples.microsoft.com/ServiceModel/Relay/")]

    public interface ICustomerContract
    {
        [OperationContract(IsOneWay = false)]
        IEnumerable<Customer> GetCustomers();

        [OperationContract(IsOneWay = false)]
        Customer GetCustomerById(string id);
    }

    public interface ICustomerChannel : ICustomerContract, IClientChannel { }
}
