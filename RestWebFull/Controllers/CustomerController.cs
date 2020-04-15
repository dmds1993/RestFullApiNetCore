using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWebFull.Dtos;
using RestWebFull.Models;
using RestWebFull.Repositories;
using RestWebFull.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Controllers
{
    [Route("api/v1/customers")]
    [Authorize(Policy = "resourcesUser")]
    public class CustomerController : Controller
    {
        private readonly ICustomerReader customerReader;
        private readonly ICreatorCustomer creatorCustomer;
        private readonly ICustomerUpdater customerUpdater;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(
            ICustomerReader customerReader,
            ICreatorCustomer creatorCustomer,
            ICustomerUpdater customerUpdater,
            ILogger<CustomerController> logger)
        {
            this.customerReader = customerReader;
            this.creatorCustomer = creatorCustomer;
            this.customerUpdater = customerUpdater;
            this.logger = logger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CustomerQueryParameters customerQueryParameters)
        {
            try
            {
                if (customerQueryParameters == null)
                    customerQueryParameters = new CustomerQueryParameters();

                logger.LogInformation("GetAll");
                var list = await customerReader.ListAll(customerQueryParameters);
                return Ok(new { Customers = list });
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "GetAll");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                logger.LogInformation($"GetById Id {id}");

                var customer = await customerReader.GetById(id);
                return Ok(new { Customer = customer });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetById");

                Console.WriteLine($"GetById ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerModel customerModel)
        {
            try
            {
                if(customerModel == null)
                    return BadRequest(new { Message = "Corpo da requição invalido" });

                if (!ModelState.IsValid)
                    return BadRequest(new { Message = "Preenchimento invalido" });


                logger.LogInformation("Create");

                await creatorCustomer.Create(customerModel);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Create");

                Console.WriteLine($"GetAll ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerModel customerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Message = "Preenchimento invalido" });

                logger.LogInformation("Update");

                customerModel.SetId(id);
                await customerUpdater.Update(customerModel);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Update");

                Console.WriteLine($"Update ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                logger.LogInformation($"Remove Id {id}");

                await customerUpdater.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Remove");

                Console.WriteLine($"Remove ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateDto> customerPatch)
        {
            try
            {
                logger.LogInformation($"Patch Id {id}");

                var customer = await customerReader.GetById(id);
                var _patch = Mapper.Map<CustomerUpdateDto>(customer);
                customerPatch.ApplyTo(_patch);
                Mapper.Map(_patch, customer);

                await customerUpdater.Patch(id, customer);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Patch Id {id}");

                Console.WriteLine($"Remove ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
