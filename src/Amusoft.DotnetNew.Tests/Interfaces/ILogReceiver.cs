namespace Amusoft.DotnetNew.Tests.Interfaces;

internal interface ILogReceiver
{
	internal void AddResult(ICommandResponse result);

	internal void AddInvocation(ICommandInvocation command);
}