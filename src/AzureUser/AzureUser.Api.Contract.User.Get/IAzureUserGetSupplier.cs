using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IAzureUserGetSupplier
{
    ValueTask<Result<AzureUserGetOut, Failure<Unit>>> GetUserAsync(AzureUserMeGetIn input, CancellationToken cancellationToken);
}