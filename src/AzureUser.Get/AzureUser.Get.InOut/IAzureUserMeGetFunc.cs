using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IAzureUserMeGetFunc
{
    ValueTask<Result<AzureUserGetOut, Failure<Unit>>> InvokeAsync(AzureUserMeGetIn input, CancellationToken cancellationToken);
}