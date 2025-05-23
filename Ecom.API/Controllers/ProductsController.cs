using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace Ecom.API.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly IImageManagementService service;
        public ProductsController(IUnitOfWork work, IMapper mapper, IImageManagementService service) : base(work, mapper)
        {
            this.service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> getAll([FromQuery]ProductParams  productParams)
        {
            try
            {
                var products = await work.ProductRepository.GetAllAsync(productParams);
                //var totalCount = await work.ProductRepository.CountAsync();
                return Ok(new Pagination<ProductDTO>(productParams.pageNumber, productParams.pageSize, products.TotalCount, products.products));
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            try
            {
                var product = await work.ProductRepository.GetByIdAsync(id, x => x.Category, x => x.Photos);
                var result = mapper.Map<ProductDTO>(product);
                if (product == null)
                {
                    return BadRequest(new ResponseAPI(400, $"Id= {id} is not found!"));

                }
                else
                {
                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }

        [Consumes("multipart/form-data")]
        [HttpPost("add-product")]
        public async Task<IActionResult> add(AddProductDTO productDTO)
        {
            try
            {
                await work.ProductRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [Consumes("multipart/form-data")]
        [HttpPut("update-product")]
        public async Task<IActionResult> update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpDelete("delete-product/{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            try
            {
                var product= await work.ProductRepository.GetByIdAsync(Id, x => x.Photos, x => x.Category );
                await work.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }



        }

    }
}