using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blip.Api.Template.Helpers;
using Blip.Api.Template.Interfaces;
using Blip.Api.Template.Models;
using Blip.Api.Template.Models.NLPContentProviderModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestEase;
using Serilog;
using SmallTalks.Core;

namespace Blip.Api.Template.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class NLPController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISmallTalksDetector _smallTalksDetector;
        private MySettings _settings { get; set; }

        public NLPController(ILogger logger, ISmallTalksDetector smallTalksDetector, MySettings settings)
        {
            _logger = logger;
            _smallTalksDetector = smallTalksDetector;
            _settings = settings;
            
        }


        [HttpPost]
        [Route("analyze")]
        public async Task<IActionResult> AnalyzeAsync([FromBody] TextRequest input, [FromHeader] string confidenceThreshold = "", [FromHeader] string cleanInputBeforeAnalysis = "true")
        {
            try
            {
                
                var validNumber = double.TryParse(confidenceThreshold, NumberStyles.Any, CultureInfo.InvariantCulture, out double currentConfidenceTreshold);

                var cleanInputParsed = bool.TryParse(cleanInputBeforeAnalysis, out bool cleanInput);
                cleanInput = cleanInputParsed ? cleanInput : true;

                currentConfidenceTreshold = currentConfidenceTreshold == 0 ? double.Parse(_settings.ContentProvider.ConfidenceThresholdDefault, NumberStyles.Any, CultureInfo.InvariantCulture) : currentConfidenceTreshold;

                var botId = _settings.BlipBotSettings.Identifier;
                var accessKey = _settings.BlipBotSettings.AccessKey;

                using (var client = RestClient.For<IContentProviderAPI>(_settings.ContentProvider.BaseUrl))
                {
                    client.AuthorizationKey = $"Key {BlipHelper.GetBotAuthorization(botId, accessKey)}";

                    var text = cleanInput ? (await _smallTalksDetector.DetectAsync(input.Text, new SmallTalks.Core.Models.SmallTalksPreProcessingConfiguration() { InformationLevel = SmallTalks.Core.Models.InformationLevel.FULL, ToLower = true, UnicodeNormalization = true })).CleanedInput : input.Text;
                    var result = await client.AnalyzeInput(text, currentConfidenceTreshold);

                    if (result.Status.Equals(ContentResultStatus.Match)&&(result.Response.IntentScore.Value > currentConfidenceTreshold))
                    {                        
                        return StatusCode(StatusCodes.Status200OK, new GenericBaseResponse { Result = result.Content.ContentText });
                    }

                    _logger.Warning($"Could not get NLP Match. Result: {result}", result);
                    
                        return StatusCode(StatusCodes.Status204NoContent, JsonConvert.SerializeObject(result));
                    
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Some error occurred on ContentProvider Analysis. Input: {input.Text}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
