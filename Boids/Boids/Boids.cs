using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boids
{
    class Boids
    {
        public int Distance;
        public int Speed;
        public Vector2 Direction;
        public int Id;
        public Sprite Sprite;
        public Texture Texture;
        List<Boids> boidsList = new List<Boids> ();
        List<Vector2> ListaForward = new List<Vector2> ();

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(Sprite.Rotation), (float)Math.Sin(Sprite.Rotation)); ;
            }
            set
            {
                Sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }
        public Boids()
        {
            Id = 0;
            Speed = 200;
            Distance = 75;
            Texture = new Texture("Assets/boid.png");
            Sprite = new Sprite(Texture.Width, Texture.Height);
            Sprite.pivot = new Vector2(Sprite.Width * 0.5f, Sprite.Height * 0.5f);
            Sprite.position = new Vector2(Map.window.MousePosition.X,Map.window.MousePosition.Y);
            Forward = (RandomGenerator.RandomForward().Normalized());
                Vector2 bigsum = new Vector2();
        }
        public void AddBoids(int i, Boids boids)
        {
            boidsList.Add(new Boids() { Id = i, });
            for (int y = 0; y < boidsList.Count; y++)
            {
                Console.WriteLine(boidsList[y].Id);
            }
        }
        public void Draw()
        {
            for (int y = 0; y < boidsList.Count; y++)
            {
                boidsList[y].Sprite.DrawTexture(Texture);
            }
        }
        public void Update()
        {
            for (int y = 0; y < boidsList.Count; y++)
            {
                Vector2 bigsum = (GetSeparation(y) + GetCohesion(y) + GetAlignment(y).Normalized());
                boidsList[y].Forward = Vector2.Lerp(boidsList[y].Forward, bigsum, Map.window.DeltaTime * 5);
                boidsList[y].Sprite.position += boidsList[y].Forward * Speed * Map.window.DeltaTime;
                boidsList[y].Sprite.position = Map.Bord(boidsList[y].Sprite.position);
            }
        }
        public Vector2 GetAlignment(int y)
        {
            Vector2 ForwardSum = Vector2.Zero;
            Vector2 disVect = Vector2.Zero;
            int counter = 1;
            for (int i = 0; i < boidsList.Count; i++)
            {
                if (y != i)
                {
                    disVect = Vector2.Zero;
                    disVect = boidsList[y].Sprite.position - boidsList[i].Sprite.position;

                    if (disVect.LengthSquared < Distance * Distance)
                    {
                        ForwardSum += boidsList[i].Forward;
                        counter++;
                    }
                }
            }

            if (counter == 1)
            {
                return boidsList[y].Forward;
            }
            ForwardSum = ForwardSum / counter;
            return ForwardSum;

        }
        public Vector2 GetCohesion(int y)
        {
            Vector2 centerOfMass = boidsList[y].Sprite.position;
            Vector2 distVect = Vector2.Zero;
            int counter = 0;
            for (int i = 0; i < boidsList.Count; i++)
            {
                if (y != i)
                {
                    distVect = Vector2.Zero;
                    distVect = boidsList[y].Sprite.position - boidsList[i].Sprite.position;

                    if (distVect.LengthSquared < Distance * Distance)
                    {
                        centerOfMass += boidsList[i].Sprite.position;
                        counter++;
                    }
                }
            }
            if (counter == 0)
            {
                return centerOfMass;
            }
            centerOfMass = centerOfMass / counter;
            return centerOfMass;
        }
        public Vector2 GetSeparation(int y)
        {
            int Distance2 = 60;
            Vector2 centerOfMass = boidsList[y].Forward;
            Vector2 distVect = Vector2.Zero;
            int counter = 0;
            for (int i = 0; i < boidsList.Count; i++)
            {
                if (y != i)
                {
                    distVect = Vector2.Zero;
                    distVect = boidsList[y].Sprite.position - boidsList[i].Sprite.position;

                    if (distVect.LengthSquared < Distance2 * Distance2)
                    {
                        centerOfMass = boidsList[i].Sprite.position;
                        counter++;
                    }
                }
            }
            if (counter == 0)
            {
                return centerOfMass;
            }
            centerOfMass = centerOfMass / counter;
            centerOfMass *= -1;
            return centerOfMass;
        }
    }
}
