using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestEase;
using Serilog;
using Take.SmartContacts.Utils;
using SmallTalks.Core;
using Blip.Api.Template.Models;
using Blip.Api.Template.Interfaces;

namespace Blip.Api.Template.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        private const string EmailRegex = "([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)";
        private readonly IDataValidator _dataValidator;
        private readonly ILogger _logger;
        private readonly IDateTimeValidator _dateValidator;
        private readonly ISmallTalksDetector _smallTalksDetector;
        private MySettings _settings;

        public ValidateController(IDataValidator dataValidator, ILogger logger, IDateTimeValidator dateValidator, ISmallTalksDetector smallTalksDetector, MySettings settings)
        {
            _dataValidator = dataValidator;
            _logger = logger;
            _dateValidator = dateValidator;
            _smallTalksDetector = smallTalksDetector;
            _settings = settings;
        }

        [HttpPost("CpfCnpj")]
        public async Task<IActionResult> ValidateCpfCnpjAsync(TextRequest request)
        {
            try
            {
                var isValid = _dataValidator.IsValid(request.Text, DataType.Cpf | DataType.Cnpj, out var resultAnalysis);
                var documentType = resultAnalysis.UsedDataType.Equals(DataType.Cpf) ? DocumentType.CPF : resultAnalysis.UsedDataType.Equals(DataType.Cnpj) ? DocumentType.CNPJ : DocumentType.None;

                return StatusCode(StatusCodes.Status200OK, new ValidateDocumentResponse { IsValid = isValid, Result = resultAnalysis.CleanedData, Type = documentType });
            }
            catch (Exception e)
            {

            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpPost("Cpf")]
        public async Task<IActionResult> ValidateCpfAsync(TextRequest request)
        {
            try
            {
                var isValid = _dataValidator.IsValid(request.Text, DataType.Cpf, out var resultAnalysis);
                var documentType = resultAnalysis.UsedDataType.Equals(DataType.Cpf) ? DocumentType.CPF : DocumentType.None;

                return StatusCode(StatusCodes.Status200OK, new ValidateDocumentResponse { IsValid = isValid, Result = resultAnalysis.CleanedData, Type = documentType });
            }
            catch (Exception e)
            {

            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpPost("Cnpj")]
        public async Task<IActionResult> ValidateCnpjAsync(TextRequest request)
        {
            try
            {
                var isValid = _dataValidator.IsValid(request.Text, DataType.Cnpj, out var resultAnalysis);
                var documentType = resultAnalysis.UsedDataType.Equals(DataType.Cnpj) ? DocumentType.CNPJ : DocumentType.None;

                return StatusCode(StatusCodes.Status200OK, new ValidateDocumentResponse { IsValid = isValid, Result = resultAnalysis.CleanedData, Type = documentType });
            }
            catch (Exception e)
            {

            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }


        [HttpPost("VehiclePlate")]
        public async Task<IActionResult> ValidateVehiclePlateAsync(TextRequest request)
        {
            try
            {
                var isValid = _dataValidator.IsValid(request.Text, DataType.CarPlate | DataType.MercosurCarPlate, out var resultAnalysis);
                var documentType = resultAnalysis.UsedDataType.Equals(DataType.CarPlate) ? DocumentType.BrazilianVehiclePlate : resultAnalysis.UsedDataType.Equals(DataType.MercosurCarPlate) ? DocumentType.MercosurVehiclePlate : DocumentType.None;

                return StatusCode(StatusCodes.Status200OK, new ValidateDocumentResponse { IsValid = isValid, Result = resultAnalysis.CleanedData, Type = documentType });
            }
            catch (Exception e)
            {

            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpPost("Name")]
        public async Task<IActionResult> ValidateNameAsync(TextRequest request)
        {

            try
            {
                var smallTalksResult = await _smallTalksDetector.DetectAsync(request.Text, new SmallTalks.Core.Models.SmallTalksPreProcessingConfiguration() { InformationLevel = SmallTalks.Core.Models.InformationLevel.FULL, ToLower = true, UnicodeNormalization = true });
                var result = TryToExtractName(smallTalksResult.CleanedInput);

                NameCheckerResponse nameCheckerResponse = null;

                if (!string.IsNullOrEmpty(result.Trim()))
                {
                    try
                    {
                        using (var client = RestClient.For<INameChecker>("https://check-name.herokuapp.com"))
                        {
                            nameCheckerResponse = await client.CheckName(result);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Error on Checker Name API", e);
                    }
                }


                result = nameCheckerResponse != null && nameCheckerResponse.Matches.Any() ? string.Join(" ", nameCheckerResponse.Matches) : result;

                try
                {
                    if (nameCheckerResponse == null || (nameCheckerResponse != null && !nameCheckerResponse.Matches.Any()))
                    {
                        return StatusCode(StatusCodes.Status200OK, new ValidateBaseResponse { IsValid = false, Result = result });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ValidateBaseResponse { IsValid = true, Result = result });
                    }

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status200OK, new ValidateBaseResponse { IsValid = false, Result = result });
                }

            }
            catch (Exception e)
            {

            }

            return StatusCode(StatusCodes.Status500InternalServerError);



        }

        [HttpPost("Email")]
        public async Task<IActionResult> ValidateEmailAsync(TextRequest request)
        {
            try
            {
                var emailRegex = new Regex(EmailRegex, RegexOptions.IgnoreCase);
                var emailMatch = emailRegex.Match(request.Text);
                var email = emailMatch?.Value?.Trim();
                var isValid = IsEmailValid(email);

                return StatusCode(StatusCodes.Status200OK, new ValidateBaseResponse { IsValid = isValid, Result = email });

            }
            catch (Exception e)
            {

            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        /// <summary>
        /// Checks if the current date and time is within sometime sent
        /// </summary>
        /// <param name="request">
        ///     Array of date and begin and end hour to check
        /// </param>
        /// <returns>Boolean indicating if now is a valid time and the current datetime as a string</returns>
        [HttpPost, Route("timeInterval/date")]
        public IActionResult IsInDateTimeInterval([FromBody] DateInterval[] request)
        {
            try
            {
                var result = _dateValidator.IsInInterval(request);
                return StatusCode(StatusCodes.Status200OK, new GenericBaseResponse() { Result = result.ToString().ToLower() });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        /// <summary>
        /// Checks whether the current time is within days and the time interval sent
        /// </summary>
        /// <param name="request">
        ///    Object of days and begin and end hour to check 
        /// </param>
        /// <returns>Boolean indicating if there was a valid time and current datetime as a string</returns>
        [HttpPost, Route("timeInterval/week")]
        public IActionResult IsInDateTimeInterval([FromBody] DaysOfWeekDateInterval[] request)
        {
            try
            {
                var result = _dateValidator.IsInInterval(request);
                return StatusCode(StatusCodes.Status200OK, new GenericBaseResponse() { Result = result.ToString().ToLower() });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        private bool IsEmailValid(string text)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(text);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string TryToExtractName(string text)
        {
            var cleanNamePattern = _settings.NameConfig.CleanNameRegexPattern;
            
            try
            {
                var inputArr = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var inputTreated = string.Empty;

                if (inputArr.Length > 0)
                {
                    var inputWithJustLetters = new StringBuilder();
                    foreach (var word in inputArr)
                    {
                        inputWithJustLetters.Append(" ").Append(new string(word.Where(char.IsLetter).ToArray()));
                    }

                    inputTreated = inputWithJustLetters.ToString().Trim();
                }

                var nameArr = inputTreated.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var value = new StringBuilder();
                //Check if can be a name
                foreach (var word in nameArr)
                {

                    if (!Regex.IsMatch(word, cleanNamePattern)  && word.Length > 1)
                    {
                        value.Append(" ").Append(word.ToLower().Substring(0, 1).ToUpper()).Append(word.ToLower().Substring(1).ToLower());
                    }

                }

                return value.ToString().Trim();
            }
            catch (Exception e)
            {
            }

            return text;
        }

    }
}
