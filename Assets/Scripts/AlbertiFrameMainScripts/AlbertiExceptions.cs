using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MissingBaseFrameException : Exception
{
    public MissingBaseFrameException() { }

    [PublicAPI]
    public MissingBaseFrameException(string message) : base(message) { }

    [PublicAPI]
    public MissingBaseFrameException(string message, Exception inner) : base(message, inner)
    {

    }
}

public class LayerNotSetException : Exception
{
    public LayerNotSetException()
    {

    }

    public LayerNotSetException(string message) : base(message) { }

    public LayerNotSetException(string message, Exception inner): base(message, inner)
    {

    }
}