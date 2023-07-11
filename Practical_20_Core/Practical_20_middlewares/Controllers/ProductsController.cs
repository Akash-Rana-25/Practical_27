using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Practical_20_middlewares.Dto;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models;

namespace Practical_20_middlewares.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<ProductsController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Get list of all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            if (products is not null)
            {
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            return NotFound();
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product is null)
            {
                _logger.LogError($"Product with Id {productId} not found when accessing {nameof(GetProductById)}");
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="productDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDetails)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(CreateProduct)} with {errors}");
                return UnprocessableEntity(ModelState);
            }
            var createdProduct = await _unitOfWork.Products.Add(_mapper.Map<Product>(productDetails));
            await _unitOfWork.Save();
            if (createdProduct is not null)
            {
                var productToReturn = _mapper.Map<ProductDto>(createdProduct);
                return CreatedAtRoute("GetProductById", new { productId = productToReturn.Id }, productToReturn);
            }
            return BadRequest();
        }

        /// <summary>
        /// Update the product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productDetails"></param>
        /// <returns></returns>
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId, UpdateProductDto productDetails)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(productId);
            if (productEntity is null)
            {
                _logger.LogError($"Product with Id {productId} not found when accessing {nameof(UpdateProduct)}");
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(UpdateProduct)} with {errors}");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(productDetails, productEntity);
            _unitOfWork.Products.Update(productEntity);
            await _unitOfWork.Save();

            return NoContent();
        }

        /// <summary>
        /// Update the product by HttpPatch request
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId, JsonPatchDocument<UpdateProductDto> patchDocument)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(productId);
            if (productEntity is null)
            {
                _logger.LogError($"Product with Id {productId} not found when accessing {nameof(UpdateProduct)}");
                return NotFound();
            }

            var productToPatch = _mapper.Map<UpdateProductDto>(productEntity);
            patchDocument.ApplyTo(productToPatch);

            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(UpdateProduct)} with {errors}");
                return UnprocessableEntity(ModelState);
            }
            if (!TryValidateModel(productToPatch))
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(UpdateProduct)} with {errors}");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(productToPatch, productEntity);
            await _unitOfWork.Save();
            return NoContent();
        }

        /// <summary>
        /// Delete the product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(productId);
            if (productEntity is null)
            {
                _logger.LogError($"Product with Id {productId} not found when accessing {nameof(DeleteProduct)}");
                return NotFound();
            }
            _unitOfWork.Products.Delete(productEntity);
            await _unitOfWork.Save();
            return NoContent();
        }
    }

}
