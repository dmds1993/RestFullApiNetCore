using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
    public class CustomerController : Controller
    {
        private readonly ICustomerReader customerReader;
        private readonly ICreatorCustomer creatorCustomer;
        private readonly ICustomerUpdater customerUpdater;

        public CustomerController(
            ICustomerReader customerReader,
            ICreatorCustomer creatorCustomer,
            ICustomerUpdater customerUpdater)
        {
            this.customerReader = customerReader;
            this.creatorCustomer = creatorCustomer;
            this.customerUpdater = customerUpdater;
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

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerModel customerModel)
        {
            try
            {
                await creatorCustomer.Create(customerModel);
                return Ok();
            }
            catch (Exception ex)
            {
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
                customerModel.SetId(id);
                await customerUpdater.Update(customerModel);
                return Ok();
            }
            catch (Exception ex)
            {
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
                await customerUpdater.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
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
                var customer = await customerReader.GetById(id);
                var _patch = Mapper.Map<CustomerUpdateDto>(customer);
                customerPatch.ApplyTo(_patch);
                Mapper.Map(_patch, customer);

                await customerUpdater.Patch(id, customer);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove ==> {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
