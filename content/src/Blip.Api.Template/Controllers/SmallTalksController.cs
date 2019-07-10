using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blip.Api.Template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmallTalks.Core;

namespace Blip.Api.Template.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SmallTalksController : ControllerBase
    {
        private readonly ISmallTalksDetector _smallTalksDetector;
        private Dictionary<string, List<string>> _smallTalkIntentions { get; set; }

        public SmallTalksController(ISmallTalksDetector smallTalksDetector, IConfiguration configuration)
        {
            _smallTalksDetector = smallTalksDetector;
            _smallTalkIntentions = configuration.GetSection("SmallTalksConfig").Get<Dictionary<string, List<string>>>();

        }

        [HttpPost]
        [Route("analyze")]
        public async Task<IActionResult> AnalyzeSmallTalksAsync([FromBody] TextRequest request)
        {
            try
            {
                var result = await _smallTalksDetector.DetectAsync(request.Text, new SmallTalks.Core.Models.SmallTalksPreProcessingConfiguration() { InformationLevel = SmallTalks.Core.Models.InformationLevel.FULL, ToLower = true, UnicodeNormalization = true });

                var smallTalksDetected = new List<string>();
                if (result.HaveCursedWords)
                {
                    smallTalksDetected.Add("CursedWords");
                }

                if (result.Matches.Any() || smallTalksDetected.Any())
                {

                    foreach (var item in result.Matches)
                    {
                        foreach(var dict in _smallTalkIntentions)
                        {
                            if(dict.Value.Contains(item.SmallTalk) && !smallTalksDetected.Contains(dict.Key))
                            {
                                smallTalksDetected.Add(dict.Key);
                            }
                        }
                        
                    }

                    return Ok(new GenericBaseResponse()
                    {
                        Result = string.Join(";", smallTalksDetected).ToLower()
                    });

                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, new GenericBaseResponse()
                    {
                        Result = ""
                    });
                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
