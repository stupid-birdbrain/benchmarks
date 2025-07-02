using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public static class Program {
    static void Main(string[] args) => BenchmarkRunner.Run<Benchmark1>();
}

public unsafe class Benchmark1 {
    private static int _value = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void IncrementAction() {
        _value++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int AddFunc(int a, int b) {
        return a + b;
    }
    
    private Action _actionDelegate;
    private Func<int, int, int> _funcDelegate;

    private delegate*<void> _actionPtr;
    private delegate*<int, int, int> _funcPtr;
    
    [GlobalSetup]
    public void Setup() {
        _actionDelegate = IncrementAction;
        _funcDelegate = AddFunc;
        _actionPtr = &IncrementAction;
        _funcPtr = &AddFunc;
    }
    
    [Benchmark(Description = "Direct Method Call")]
    public void DirectCallAction() {
        IncrementAction();
    }

    [Benchmark(Description = "Action Delegate Invocation")]
    public void ActionInvocation() {
        _actionDelegate();
    }

    [Benchmark(Description = "Function Pointer Invocation (void)")]
    public void FunctionPointerAction() {
        _actionPtr();
    }

    [Benchmark(Description = "Direct Method Call")]
    public int DirectCallFunc() {
        return AddFunc(1, 2);
    }

    [Benchmark(Description = "Func Delegate Invocation")]
    public int FuncInvocation() {
        return _funcDelegate(1, 2);
    }

    [Benchmark(Description = "Function Pointer Invocation")]
    public int FunctionPointerFunc() {
        return _funcPtr(1, 2);
    }
}