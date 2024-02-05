using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Realworld.Api.Dto {
    public record RequestEnvelope<T> where T : class
    {
        [Required] [FromBody] public T Body { get; init; } = default!;
    }

}

