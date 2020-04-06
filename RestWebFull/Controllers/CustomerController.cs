using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWebFull.Repositories;
using RestWebFull.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Controllers
{
    [Route("api/v1/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerReader customerReader;
        public CustomerController(ICustomerReader customerReader)
        {
            this.customerReader = customerReader;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await customerReader.ListAll();
                return Ok(new { Customers = list });
            }
            catch(Exception ex)
            {
                Console.WriteLine($"GetAll ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var customer = await customerReader.GetById(id);
                return Ok(new { Customer = customer });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAll ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
