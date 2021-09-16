using UnityEngine;
//we are using unity libaries

    public class GPUGraph : MonoBehaviour
    {//Naming the script and making it public so everything can call it 
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

        [SerializeField]
        ComputeShader computeShader;
        //Giving the code possibility to change between which shader it uses 

        [SerializeField]
        Material material;

        [SerializeField]
        Mesh mesh;

        ComputeBuffer positionBuffer;
    void OnEnable()
    {
        positionBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }
    void OnDisable()
    {
        positionBuffer.Release();
        positionBuffer = null;
    }

    static readonly int positionsId = Shader.PropertyToID("_Positions"),
            resolutionId = Shader.PropertyToID("_Resolution"),
            stepId = Shader.PropertyToID("_Step"),
            timeId = Shader.PropertyToID("_Time");
        //Adding static readonly field to store identifiers from our shader script 

        //Serialize field is method to add visual components to the code that you can edit or change in unity editor 
        float duration;
        bool transitioning;
        FunctionLibary.FunctionName transitionFunction;
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
            //we are calling function libary method to switcht between functions in it 
            UpdateFunctionOnGPU();
        }
        void PickNextFunction()
        {
            function = transitionMode == TransitionMode.Cycle ?
        FunctionLibary.GetNextFunctionName(function) :
        FunctionLibary.GetRandomFunctionOtherThan(function);
        }
        //Adding second mode to cycling between functions form function libary Random 
        //Under this line we are starting to make it use GPU
        //We have allocated space in GPU for the code to run with it instead of CPU 
        //OnEnable method prevents objects assined to this method from disapearing on hotreaload 
        void UpdateFunctionOnGPU()
        {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetBuffer(0, positionsId, positionBuffer);
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
        material.SetBuffer(positionsId, positionBuffer);
        material.SetFloat(stepId, step);
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionBuffer.count);
        }
    } 

