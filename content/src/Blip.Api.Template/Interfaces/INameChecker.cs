using Blip.Api.Template.Models;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blip.Api.Template.Interfaces
{
    public interface INameChecker : IDisposable
    {
        [Get("/verify/{name}")]
        Task<NameCheckerResponse> CheckName([Path] string name);
    }
}
