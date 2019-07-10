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
        private readonly IConfiguration _configuration;
        private readonly ISmallTalksDetector _smallTalksDetector;

        private string _contentProviderBaseUrl { get; set; }
        private double _confidenceThresholdDefault { get; set; }
        public NLPController(ILogger logger, IConfiguration configuration, ISmallTalksDetector smallTalksDetector)
        {
            _logger = logger;
            _configuration = configuration;
            _contentProviderBaseUrl = _configuration.GetSection("ContentProvider:BaseUrl").Value;
            _smallTalksDetector = smallTalksDetector;
            _confidenceThresholdDefault = double.Parse(_configuration.GetSection("ContentProvider:confidenceThresholdDefault").Value, NumberStyles.Any, CultureInfo.InvariantCulture);
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

                currentConfidenceTreshold = currentConfidenceTreshold == 0 ? _confidenceThresholdDefault : currentConfidenceTreshold;

                var botId = _configuration.GetSection("BlipConfigurations:BotIdentifier").Value;
                var accessKey = _configuration.GetSection("BlipConfigurations:BotAccessKey").Value;

                using (var client = RestClient.For<IContentProviderAPI>(_contentProviderBaseUrl))
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
