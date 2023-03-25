namespace Interfaces
{
    public interface IBusStopArea
    {
        public void AddPassenger(IPassenger newPassenger);
        public IPassenger RemovePassenger();
    }
}