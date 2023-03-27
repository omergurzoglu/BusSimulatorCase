using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Passengers
{
    /// <summary>
    /// Class responsible for spawning 
    /// </summary>
    public class RandomPassengerSpawnStrategy : IPassengerSpawnStrategy
    {
        public void SpawnPassengers(Transform busStopTransform, Func<Passenger> passengerFactory, List<Passenger> passengers)
        {
            int randomSpawnCount = Random.Range(1, 4);
            for (int i = 0; i < randomSpawnCount; i++)
            {
                var newPassenger = passengerFactory();
                newPassenger.transform.position = new Vector3(Random.Range(1, 5), 0, Random.Range(1, 5)) + busStopTransform.position;
                newPassenger.passengerInOrOutState = Passenger.PassengerInOrOut.PassengerIn;
                passengers.Add(newPassenger);
            }
        }
    }
}