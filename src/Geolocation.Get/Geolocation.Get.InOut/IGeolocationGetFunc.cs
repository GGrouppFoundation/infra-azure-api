using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IGeolocationGetFunc
{
    ValueTask<Result<GeolocationGetOut, Failure<GeolocationGetFailureCode>>> InvokeAsync(
        GeolocationGetIn input, CancellationToken cancellationToken);
}