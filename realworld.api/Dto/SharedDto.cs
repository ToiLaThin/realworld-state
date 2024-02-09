using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Realworld.Api.Dto {

    /// <summary>
    /// This is not needed => it make the request json to be {"body": { ... }} instead of { ... } => cause api test give wrong result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record RequestEnvelope<T> where T : class
    {
        [Required] [FromBody] public T Body { get; init; } = default!;
    }

}

