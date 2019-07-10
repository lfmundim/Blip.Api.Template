using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Take.Blip.Client.Extensions.Bucket;

namespace Blip.Api.Template.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BucketExampleController : ControllerBase
    {
        public ILogger _logger { get; set; }
        public IBucketExtension _bucket { get; set; }

        public BucketExampleController(ILogger logger, IBucketExtension bucket)
        {
            _logger = logger;
            _bucket = bucket;
        }


        [HttpGet("bucketExample/{text}")]
        public async Task<IActionResult> BucketExampleAsync(string text)
        {

            try
            {
                await _bucket.SetAsync("bucketVariableExample", new PlainText { Text = "bucketVariableValue" });
                var testBucket = await _bucket.GetAsync<PlainText>("bucketVariableExample");

                _logger.Debug("Variable on bucket: {bucketVariableExample}", testBucket);
                return Ok($"testBucket");
            }

            catch (Exception e)
            {
                _logger.Error(e, "Process failed on endpoint BucketExample. EndpointParameter: {endpointParameter}", text);

            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

    }
}
