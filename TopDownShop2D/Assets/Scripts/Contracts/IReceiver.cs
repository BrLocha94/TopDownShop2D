namespace Project.Contracts
{
    public interface IReceiver<T>
    {
        void ReceiveUpdate(T updatedValue);
    }
}