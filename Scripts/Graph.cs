using UnityEngine;
//we are using unity libaries
namespace AA2763
{
    public class Graph : MonoBehaviour
    {//Naming the script and making it public so everything can call it 
        [SerializeField]
        Transform pointPrefab;
        //telling unity that were are talking about specific game object
        [SerializeField, Range(10, 150)]
        int resolution = 10;
        //Adding more option to add more cubes in Unity editor instead of doing it in code
        [SerializeField]
        FunctionLibary.FunctionName function;

        public enum TransitionMode { Cycle, Random }

        [SerializeField]
        TransitionMode transitionMode;

        [SerializeField, Min(0f)]
        float functionDuration = 1f, transitionDuration = 1f;

        //Serialize field is method to add visual components to the code that you can edit or change in unity editor 
        Transform[] points;
        float duration;

        bool transitioning;
        FunctionLibary.FunctionName transitionFunction;
        private void Awake()
        {
            float step = 2f / resolution;
            var scale = Vector3.one * step;
            //defining variable and floats so my code can work 
            points = new Transform[resolution * resolution];
            //Array that has length = to resolution variable
            for (int i = 0; i < points.Length; i++)
            //For loop that contains parameter for how long this code loop i = starting position of cube we are copiyng, i < resolution tells to loop as many times as what resolution is set to, i++ ads 1 to i each loop
            //x++ does same for x
            {
                Transform point = Instantiate(pointPrefab);
                //Changes the positions of each instance of
                //the prefabs to be between -1 and 1
                point.localScale = scale;
                //This changes the distance between each prefab variable scale does the calculation 
                point.SetParent(transform, false);
                //This set all the prefabs as childs of first one generated 
                points[i] = point;
            }
        }

        void Update()
        {
            duration += Time.deltaTime;
            if (transitioning)
            {
                if (duration >= functionDuration)
                {
                    duration -= transitionDuration;
                    transitioning = false;
                }
            }
            else if (duration >= functionDuration)
            {
                duration -= functionDuration;
                transitioning = true;
                transitionFunction = function;
                PickNextFunction();
            }
            if (transitioning)
            {
                UpdateFunctionTransition();
            }
            else
            {
                UpdateFunction();
            }
            //we are calling function libary method to switcht between functions in it 
        }
        void PickNextFunction()
        {
            function = transitionMode == TransitionMode.Cycle ?
        FunctionLibary.GetNextFunctionName(function) :
        FunctionLibary.GetRandomFunctionOtherThan(function);
        }
        //Adding second mode to cycling between functions form function libary Random 

        void UpdateFunction()
        {
            FunctionLibary.Function f = FunctionLibary.GetFunction(function);
            //searches for the function from functionlibary
            float time = Time.time;
            //add time attripute to function
            float step = 2f / resolution;
            float v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, time);

                //this whole thing animates the function to move with time. FunctionLibary is libary to add different mathfunctions to this code instead of having to write them my self over and over again
                //That libary is very basic as i have written it my self 
            }
        }
        void UpdateFunctionTransition()
        {
            FunctionLibary.Function
                from = FunctionLibary.GetFunction(transitionFunction),
                to = FunctionLibary.GetFunction(function);
            float progress = duration / transitionDuration;
            //searches for the function from functionlibary
            float time = Time.time;
            //add time attripute to function
            float step = 2f / resolution;
            float v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = FunctionLibary.Morph(u, v, time, from, to, progress);

                //this whole thing animates the function to move with time. FunctionLibary is libary to add different mathfunctions to this code instead of having to write them my self over and over again
                //That libary is very basic as i have written it my self 
            }
        }
    } 
}
