open System

type private TypedTracer<'a>() =
    static member val event = Event<'a>()

let emit<'trace> x =
    TypedTracer<'trace>.event.Trigger(x)

let subscribe<'trace> f =
    TypedTracer<'trace>.event.Publish |> Observable.subscribe f

type private TypedObserver<'group, 'trace>() =
    static member val disposable : IDisposable ref = ref null

let link<'group, 'trace> f =
    let disposable = TypedObserver<'group, 'trace>.disposable
    lock disposable <| fun () ->
        if !disposable <> null then (!disposable).Dispose()
        disposable := subscribe<'trace>(f)

let unlink<'group, 'trace>() =
    let disposable = TypedObserver<'group, 'trace>.disposable
    lock disposable <| fun () ->
        if !disposable <> null then (!disposable).Dispose()
        disposable := null

type Division = Division of int * int

let some_complex_function x y =
  try
    emit <| Division(x, y)
    x / y
  with ex ->
    emit<exn> ex
    0

type Logger = Logger
link<Logger, exn> (fun ex -> printf "%O" ex)

some_complex_function 2 2
some_complex_function 42 0