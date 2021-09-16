using UnityEngine;
using static UnityEngine.Mathf;
//adding different libaries to make life lot easier aka we dont need to write libary names to begining of each command from those libaries

    public static class FunctionLibary
    //Static keyword tells unity this is not object scriptm, public allows us to use this script and call it or parts of it to any other script we are using.
    //AKA this is Libary for mathematical functions 
    {
        public delegate Vector3 Function(float u, float v, float t);
        public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus, }

        static Function[] functions = { Wave, MultiWave, Ripple, Sphere, Torus, };
        public static Function GetFunction(FunctionName name)
        {
            return functions[(int)name];
        }
        public static Vector3 Wave(float u, float v, float t)
        //Addibng method named Wave that has static modifier to make it work at class level instead of in instances, public to make it open to everything, Float to make it Floating point number 
        //float x and t are parameter that can be use in mathematical funtions 
        {
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (u + v + t));
            p.z = v;
            return p;
            //Function to calculate and return sine wave 
        }
        public static Vector3 MultiWave(float u, float v, float t)
        {
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (u + 0.5f * t));
            p.y = 0.5f * Sin(2f * PI * (v + t)) / 2f;
            p.y = Sin(PI * (u + v + 0.25f * t));
            p.y *= 1f / 2.5f;
            p.z = v;
            return p;
            //Function that makes sine wave times something i dont actually know 
        }
        public static Vector3 Ripple(float u, float v, float t)
        {

            float d = Sqrt(u * u + v * v);
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (4f * d - t));
            p.y /= 1f + 10f * d;
            p.z = v;
            return p;
            //Function that makes ripple effect 
        }
        public static Vector3 Sphere(float u, float v, float t)
        {
            float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v * t));
            float s = r * Cos(0.5f * PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r * Sin(PI * 0.5f * v);
            p.z = s * Cos(PI * u);
            return p;
            //Function for pulsating sphere 
        }
        public static Vector3 Torus(float u, float v, float t)
        {
            float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
            float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
            float s = r1 + r2 * Cos(PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r2 * Sin(PI * v);
            p.z = s * Cos(PI * u);
            return p;
            //Function to build Torus 
        }
        public static FunctionName GetNextFunctionName(FunctionName name)
        {
            return (int)name < functions.Length - 1 ? name + 1 : 0;
            if ((int)name < functions.Length - 1)
            {
                return name + 1;
            }
            else
            {
                return 0;
            }
            //Function that seds functions from this libary forward at specific intervals when called 
        }
        public static FunctionName GetRandomFunctionOtherThan(FunctionName name)
        {
            var choice = (FunctionName)Random.Range(1, functions.Length);
            return choice == name ? 0 : choice;
        }
        //Function that send forward random function form this libary when called 
        public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
        {
            return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
            //Lerp = Linear interpolation provides constant speed transition between functions
        }
    } 
