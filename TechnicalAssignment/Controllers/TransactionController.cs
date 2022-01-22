using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Utility.Common;
using Utility.Response;

namespace TechnicalAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private IHostEnvironment _environment;
        public TransactionController(ITransactionService transactionService, IMapper mapper, IHostEnvironment environment)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpPost("InsertTransaction")]
        public IActionResult InsertTransaction([FromForm] IFormFile file)
        {
            UploadFileProcess(file);
            var result = ReadXMLBasedUpload(file.FileName);
           
            var mappedSchool = _mapper.Map<Transaction>(result);
            _transactionService.InsertTransaction(mappedSchool);
            return Ok(
                new { Status = StatusResponse.OK }
            );
        }

        [HttpGet("GetAllTransaction")]
        public IActionResult GetAllSchool([FromQuery] Pager page)
        {
            var result = _transactionService.GetAllTransaction(page);
            return Ok(result);
        }

        [HttpGet("SampleReturn")]
        public IActionResult SampleReturn()
        {
            var result = "Success";
            return Ok(result);
        }
        public Transaction ReadXMLBasedUpload(string fileName)
        {
            var result = new Transaction();
            XmlDocument doc = new XmlDocument();
            doc.Load(string.Concat(this._environment.ContentRootPath, "/UploadedFile/"));
            foreach(XmlNode node in doc.SelectNodes("Transactions"))
            {
                result.TransactionId=node["id"].InnerText;
                foreach(XmlNode node1 in node.SelectNodes("PaymentDetails"))
                {
                    result.Amount = Convert.ToDouble(node["Amount"].InnerText);
                    result.CurrencyCode =node["CurrencyCode"].InnerText;
                }
                result.TransactionDate =DateTime.ParseExact(node["TransactionDate"].InnerText, "yyyy-MM-ddThh:mm:ss", null);
                result.TransactionStatus = Convert.ToInt32(node["Status"].InnerText);
            }

            return result;
        }
        
        public void UploadFileProcess(IFormFile file)
        {
            var dir = _environment.ContentRootPath;
            using (var fileStream = new FileStream(Path.Combine(dir+"/UploadedFile",file.FileName), FileMode.Open))
            {
                file.CopyTo(fileStream);
            }
        }
    }
}
