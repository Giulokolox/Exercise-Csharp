﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    static class PhysicsMngr
    {
        static List<RigidBody> items;

        static PhysicsMngr()
        {
            items = new List<RigidBody>();
        }

        public static void AddItem(RigidBody rb)
        {
            items.Add(rb);
        }

        public static void RemoveItem(RigidBody rb)
        {
            items.Remove(rb);
        }

        public static void Update()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].IsActive)
                {
                    items[i].Update();
                }
            }
        }

        public static void CheckCollisions()
        {
            // Check all the items (from the first) except the last one
            for (int i = 0; i < items.Count - 1; i++)
            {
                if(items[i].IsActive && items[i].IsCollisionAffected)
                {
                    // Check each time the next item (from the second to the last one)
                    for (int j = i + 1; j < items.Count; j++)
                    {
                        if(items[j].IsActive && items[j].IsCollisionAffected)
                        {

                            bool firstCheck = items[i].CollisionTypeMatches(items[j].Type);
                            bool secondCheck = items[j].CollisionTypeMatches(items[i].Type);

                            // If items collides (we check one of them, here it's the same)
                            if ((firstCheck || secondCheck) && items[i].Collides(items[j]))
                            {
                                // Now instead we call OnCollide on both GameObject
                                if (firstCheck)
                                {
                                    items[i].GameObject.OnCollide(items[j].GameObject); 
                                }

                                if (secondCheck)
                                {
                                    items[j].GameObject.OnCollide(items[i].GameObject); 
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ClearAll()
        {
            items.Clear();
        }
    }
}