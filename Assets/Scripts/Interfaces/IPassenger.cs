using Objects.Bus;

namespace Interfaces
{
    public interface IPassenger
    {
        public void SetBusStopTargetForPassenger(BusStopArea busStopArea);
        public void EnterBus();
        public void ExitBus();
    }
}