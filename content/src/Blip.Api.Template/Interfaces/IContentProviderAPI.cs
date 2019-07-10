using Blip.Api.Template.Models.NLPContentProviderModels;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blip.Api.Template.Interfaces
{
    public interface IContentProviderAPI : IDisposable
    {
        [Header("Authorization")]
        string AuthorizationKey { get; set; }

        [Get("/answer")]
        Task<ContentAnalysisResult> AnalyzeInput([Query] string input, [Query] double confidenceThreshold);
    }
}
