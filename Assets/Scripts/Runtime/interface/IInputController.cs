using System;

namespace Nomtec
{
    public interface IInputController
    {
        event Action onClick;
        event Action onCancel;
    }
}
