using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IGeolocationGetSupplier
{
    ValueTask<Result<GeolocationGetOut, Failure<GeolocationGetFailureCode>>> GetGeolocationAsync(
        GeolocationGetIn input, CancellationToken cancellationToken);
}