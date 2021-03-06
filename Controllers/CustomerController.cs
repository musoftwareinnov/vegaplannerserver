using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using System.Globalization;
using vega.Extensions.DateTime;
using Microsoft.Extensions.Options;
using vega.Core.Models.Settings;
using vega.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using vegaplanner.Core.Models.Security.Helpers;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/customers")]
    public class CustomerController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IStaticDataRepository staticDataRepository;
        public CustomerController(IMapper mapper, 
                                  ICustomerRepository customerRepository, 
                                  IUnitOfWork unitOfWork,
                                  IStaticDataRepository staticDataRepository)
        {
            this.unitOfWork = unitOfWork;
            this.customerRepository = customerRepository;
            this.staticDataRepository = staticDataRepository;
            this.mapper = mapper;
        }
   

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)  
        {
            var customer = await customerRepository.GetCustomer(id);

            if (customer == null)
                return NotFound("Customer");

            var result = mapper.Map<Customer, CustomerResource>(customer);

            return Ok(result);
        } 

        [HttpGet]
        public async Task<QueryResultResource<CustomerResource>> GetCustomers(CustomerQueryResource filterResource)     
        {
            var filter = mapper.Map<CustomerQueryResource, CustomerQuery>(filterResource);
            
            var queryResult = await customerRepository.GetCustomers(filter);
    
            return mapper.Map<QueryResult<Customer>, QueryResultResource<CustomerResource>>(queryResult);             
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerResource customerResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = mapper.Map<CustomerResource, Customer>(customerResource);

            if (customerRepository.CustomerExists(customer))
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Username already taken.", ModelState));
            }

            customer.CustomerContact.CustomerTitle=staticDataRepository.GetTitle(customerResource.TitleId);
            customerRepository.Add(customer);

            await unitOfWork.CompleteAsync();

            customer = await customerRepository.GetCustomer(customer.Id);

            var result = mapper.Map<Customer, CustomerResource>(customer);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerResource customerResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerctx = await customerRepository.GetCustomer(customerResource.Id);
            mapper.Map<CustomerResource, Customer>(customerResource,customerctx) ;

            customerctx.CustomerContact.CustomerTitle=staticDataRepository.GetTitle(customerResource.TitleId);
            customerRepository.Update(customerctx);

            await unitOfWork.CompleteAsync();

            //var customer = await customerRepository.GetCustomer(customerctx.Id);

            var result = mapper.Map<Customer, CustomerResource>(customerctx);
 
            return Ok(result);
        } 
    }
}