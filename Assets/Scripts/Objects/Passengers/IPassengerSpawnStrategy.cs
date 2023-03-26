using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Passengers
{
    public interface IPassengerSpawnStrategy
    {
        void SpawnPassengers(Transform busStopTransform, Func<Passenger> passengerFactory, List<Passenger> passengers);
    }
}