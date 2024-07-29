using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Fast3D;
using OpenTK;

namespace Progetto_3D
{
    class Program
    {
        // Variables
        static float cameraMovSpeed = 20.0f;
        static float cameraRotSpeed = 10.0f;

        static Vector2 currentMousePosition;
        static Vector2 mouseRawPosition;
        static Vector3 direction;
        static Vector3 lastRotation;
        static Vector3 lastForward;
        static Vector3 lerp;

        static Vector3 look;
        static float lookAtan;
        static float preWheel;

        static Mesh3[] mesh;

        static PerspectiveCamera camera;

        static void Main(string[] args)
        {
            // WINDOW SETUP
            Window window = new Window(800, 600, "Ivysaur");
            currentMousePosition = window.RawMousePosition;
            window.EnableDepthTest();
            window.CullBackFaces();
            // Camera
            camera = new PerspectiveCamera(new Vector3(0.0f, 7.0f, 50.0f), new Vector3(0.0f, 0.0f, 0.0f), 60.0f, 0.1f, 1000.0f);
            // Lights
            DirectionalLight light = new DirectionalLight(new Vector3(-1.0f, 0.5f, 0.0f));
            Vector3 ambientLight = new Vector3(0.3f);


            // ASSETS INITIALIZATION

            // Textures
            Texture Ivysaur = new Texture("Assets/Final_Pokemon_Diffuse.png");
            Texture wallDiffuse = new Texture("Assets/cartoon_grass.png");

            // OBJECTS

            mesh = SceneImporter.LoadMesh("Assets/Pokemon.obj");
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i] = SceneImporter.LoadMesh("Assets/Pokemon.obj")[i];
                mesh[i].Position3 = new Vector3(0.0f, 0.0f, 20.0f);
                mesh[i].Scale3 = new Vector3(3.0f);
            }
            Mesh3 floor = SceneImporter.LoadMesh("Assets/quad.obj")[0];
            floor.Position3 = new Vector3(0.0f, 0.0f, 20.0f);
            floor.EulerRotation3 = new Vector3(270.0f, 0.0f, 0.0f);
            floor.Scale3 = new Vector3(40, 40, 0);
            // Materials
            Material wallMaterial = new Material();
            wallMaterial.Ambient = ambientLight;
            wallMaterial.Diffuse = wallDiffuse;
            wallMaterial.Lights = new Light[] { light };

            Material r2d2Material = new Material();
            r2d2Material.Ambient = ambientLight;
            r2d2Material.Diffuse = wallDiffuse;
            r2d2Material.Lights = new Light[] { light };

            preWheel = 0;





            // MAIN LOOP
            while (window.IsOpened)
            {
                // Show FPS on Window Title Bar
                window.SetTitle($"Materials   -   FPS: {1f / window.DeltaTime}");
                window.SetVSync(false);

                // INPUT--------------------------------------------------------
                CameraInput(window, camera);



                // UPDATE-------------------------------------------------------               



                // DRAW---------------------------------------------------------
                //wall.DrawColor(purple);
                //wall.DrawPhong(yellow, light, new Vector3(1.0f, 1.0f, 1.0f));
                //wall.DrawTexture(wallNormal);
                floor.DrawPhong(wallMaterial);

                //r2d2.DrawCel(yellow, light, ambientLight);
                //r2d2.DrawTexture(r2d2Diffuse);
                for (int i = 0; i < mesh.Length; i++)
                {
                    mesh[i].DrawTexture(Ivysaur);
                }

                window.Update();
            }
        }

        static void CameraInput(Window window, PerspectiveCamera camera)
        {
            //Forward(Z Axis)
            if (window.GetKey(KeyCode.W))
            {
                //camera.Position3 += camera.Forward * cameraMovSpeed * window.DeltaTime;
                for (int i = 0; i < mesh.Length; i++)
                {
                    //mesh[i].Position3 -= new Vector3(0.0f, 0.0f, 1.0f) * cameraMovSpeed * window.DeltaTime
                    //mesh[i].Rotation3 = new Vector3(0, (float)Math.Atan2(-camera.Forward.X, -camera.Forward.Z), 0);
                    lastRotation = mesh[i].Rotation3;
                    //Vector3.Lerp(lastForward, new Vector3(-camera.Forward.X, 0, -camera.Forward.Z), window.DeltaTime * 5);
                    mesh[i].Rotation3 = Vector3.Lerp(lastRotation, new Vector3(0, (float)Math.Atan2(-camera.Forward.X, -camera.Forward.Z), 0), window.DeltaTime * 5);
                    direction = mesh[i].Position3 - camera.Position3;
                    direction.Normalize();
                    mesh[i].Position3 += new Vector3(direction.X, 0, direction.Z) * cameraMovSpeed * window.DeltaTime;
                }
                camera.Position3 += new Vector3(direction.X, 0, direction.Z) * cameraMovSpeed * window.DeltaTime;
            }
            else if (window.GetKey(KeyCode.S))
            {
                //camera.Position3 -= camera.Forward * cameraMovSpeed * window.DeltaTime;
                for (int i = 0; i < mesh.Length; i++)
                {
                    //mesh[i].Position3 -= new Vector3(0.0f, 0.0f, 1.0f) * cameraMovSpeed * window.DeltaTime
                    lastRotation = mesh[i].Rotation3;
                    mesh[i].Rotation3 = Vector3.Lerp(lastRotation, new Vector3(0, (float)Math.Atan2(camera.Forward.X, camera.Forward.Z), 0), window.DeltaTime * 5);
                    direction = mesh[i].Position3 - camera.Position3;
                    direction.Normalize();
                    mesh[i].Position3 -= new Vector3(direction.X, 0, direction.Z) * cameraMovSpeed * window.DeltaTime;
                }
                camera.Position3 -= new Vector3(direction.X, 0, direction.Z) * cameraMovSpeed * window.DeltaTime;
            }
            // Right (X Axis)
            if (window.GetKey(KeyCode.A))
            {
                //camera.Position3 -= camera.Right * cameraMovSpeed * window.DeltaTime;
                for (int i = 0; i < mesh.Length; i++)
                {
                    //mesh[i].Position3 -= new Vector3(0.0f, 0.0f, 1.0f) * cameraMovSpeed * window.DeltaTime
                    lastRotation = mesh[i].Rotation3;
                    mesh[i].Rotation3 = Vector3.Lerp(lastRotation, new Vector3(0, (float)Math.Atan2(camera.Forward.X, camera.Forward.Z) - (float)Math.PI / 2, 0), window.DeltaTime * 5);
                    direction = mesh[i].Position3 - camera.Position3;
                    direction.Normalize();
                    mesh[i].Position3 -= new Vector3(-direction.Z, 0, direction.X) * cameraMovSpeed * window.DeltaTime;
                }
                camera.Position3 -= new Vector3(-direction.Z, 0, direction.X) * cameraMovSpeed * window.DeltaTime;
            }
            else if (window.GetKey(KeyCode.D))
            {
                for (int i = 0; i < mesh.Length; i++)
                {
                    //mesh[i].Position3 -= new Vector3(0.0f, 0.0f, 1.0f) * cameraMovSpeed * window.DeltaTime
                    //mesh[i].Rotation3 = new Vector3(0, (float)Math.Atan2(-camera.Forward.X, -camera.Forward.Z) - (float)Math.PI / 2, 0);
                    lastRotation = mesh[i].Rotation3;
                    mesh[i].Rotation3 = Vector3.Lerp(lastRotation, new Vector3(0, (float)Math.Atan2(camera.Forward.X, camera.Forward.Z) + (float)Math.PI / 2, 0), window.DeltaTime * 5);
                    direction = mesh[i].Position3 - camera.Position3;
                    direction.Normalize();
                    mesh[i].Position3 -= new Vector3(direction.Z, 0, -direction.X) * cameraMovSpeed * window.DeltaTime;
                }
                camera.Position3 -= new Vector3(direction.Z, 0, -direction.X) * cameraMovSpeed * window.DeltaTime;
            }

            if (window.MouseWheel != preWheel)
            {                           
                if (preWheel > window.MouseWheel)
                {
                    camera.Position3 -= new Vector3(0.0f, 0.0f, 20.0f) * cameraMovSpeed * window.DeltaTime;
                }
                else
                    camera.Position3 += new Vector3(0.0f, 0.0f, 20.0f) * cameraMovSpeed * window.DeltaTime;
                preWheel = window.MouseWheel;
            }
        

            ////Up(Y Axis)
            //if (window.GetKey(KeyCode.Q))
            //{
            //    camera.Position3 -= camera.Up * cameraMovSpeed * window.DeltaTime;
            //}
            //else if (window.GetKey(KeyCode.E))
            //{
            //    camera.Position3 += camera.Up * cameraMovSpeed * window.DeltaTime;
            //}

            // Escape
            if (window.GetKey(KeyCode.Esc))
            {
                window.Exit();
            }

            Vector2 deltaMouse = (window.RawMousePosition - mouseRawPosition) * cameraRotSpeed * window.DeltaTime;
            mouseRawPosition = window.RawMousePosition;
            //camera.EulerRotation3 += new Vector3(0.0f, deltaMouse.X, 0.0f) * 3;

            if (currentMousePosition.X < mouseRawPosition.X)
            {
                look = mesh[8].Position3 - camera.Position3;
                lookAtan = (float)Math.Atan2(look.Z, look.X);
                currentMousePosition.X = mouseRawPosition.X;
                camera.Position3 -= camera.Right * cameraMovSpeed * window.DeltaTime * 5.0f;
                camera.Rotation3 = new Vector3(0.0f, (lookAtan - (float)Math.PI / 2), 0.0f);
            }
            else if (currentMousePosition.X > mouseRawPosition.X)
            {
                look = mesh[8].Position3 - camera.Position3;
                lookAtan = (float)Math.Atan2(look.Z, look.X);
                currentMousePosition.X = mouseRawPosition.X;
                camera.Position3 += camera.Right * cameraMovSpeed * window.DeltaTime * 5.0f;
                camera.Rotation3 = new Vector3(0.0f, (lookAtan - (float)Math.PI / 2), 0.0f);
            }
        }

        //private static Vector3 CalculateCenterPivot(Mesh3 mesh)
        //{
        //    Vector3 mins = new Vector3(float.MaxValue);
        //    Vector3 maxs = new Vector3(float.MinValue);

        //    for (int i = 0; i < mesh.v.Length; i += 3)
        //    {
        //        float x = mesh.v[i + 0];
        //        float y = mesh.v[i + 1];
        //        float z = mesh.v[i + 2];

        //        if (mins.X > x) mins.X = x;
        //        if (mins.Y > y) mins.Y = y;
        //        if (mins.Z > z) mins.Z = z;

        //        if (maxs.X < x) maxs.X = x;
        //        if (maxs.Y < y) maxs.Y = y;
        //        if (maxs.Z < z) maxs.Z = z;
        //    }

        //    return (maxs + mins) / 2.0f;
        //}
    }
}
