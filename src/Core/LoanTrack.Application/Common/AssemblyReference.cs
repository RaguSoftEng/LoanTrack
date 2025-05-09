using System.Reflection;

namespace LoanTrack.Application.Common;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
