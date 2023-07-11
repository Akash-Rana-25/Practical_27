using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Practical_20_middlewares.Dto;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models;

namespace Practical_20_middlewares.Controllers
{
    

        [ApiController]
        [Route("api/categories")]
        public class CategoriesController : ControllerBase
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<CategoriesController> _logger;

            public CategoriesController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            /// <summary>
            /// Get list of all categories
            /// </summary>
            /// <returns></returns>
            [HttpGet]
            public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                if (categories is not null)
                {
                    return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
                }
                return NotFound();
            }

            /// <summary>
            /// Get category by id
            /// </summary>
            /// <param name="categoryId"></param>
            /// <returns></returns>
            [HttpGet("{categoryId}", Name = "GetCategoryById")]
            public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid categoryId)
            {
                var categoty = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (categoty is null)
                {
                    _logger.LogError($"Category with Id {categoryId} not found when accessing {nameof(GetCategoryById)}");
                    return NotFound();
                }
                return Ok(_mapper.Map<CategoryDto>(categoty));
            }

            /// <summary>
            /// Create a new product
            /// </summary>
            /// <param name="categoryDetails"></param>
            /// <returns></returns>
            [HttpPost]
            public async Task<IActionResult> CreateCategory(CreateCategoryDto categoryDetails)
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                    _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(CreateCategory)} with {errors}");
                    return UnprocessableEntity(ModelState);
                }
                var createdCategory = await _unitOfWork.Categories.Add(_mapper.Map<Category>(categoryDetails));
                await _unitOfWork.Save();
                if (createdCategory is not null)
                {
                    var categoryToReturn = _mapper.Map<CategoryDto>(createdCategory);
                    return CreatedAtRoute("GetCategoryById", new { categoryId = categoryToReturn.Id }, categoryToReturn);
                }
                return BadRequest();
            }

            /// <summary>
            /// Update the category
            /// </summary>
            /// <param name="categoryId"></param>
            /// <param name="categoryDetails"></param>
            /// <returns></returns>
            [HttpPut("{categoryId}")]
            public async Task<IActionResult> UpdateCategory(Guid categoryId, UpdateCategoryDto categoryDetails)
            {
                var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (categoryEntity is null)
                {
                    _logger.LogError($"Category with Id {categoryId} not found when accessing {nameof(UpdateCategory)}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                    _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(UpdateCategory)} with {errors}");
                    return UnprocessableEntity(ModelState);
                }
                _mapper.Map(categoryDetails, categoryEntity);
                _unitOfWork.Categories.Update(categoryEntity);
                await _unitOfWork.Save();

                return NoContent();
            }

            /// <summary>
            /// Update the category by HttpPatch request
            /// </summary>
            /// <param name="categoryId"></param>
            /// <param name="patchDocument"></param>
            /// <returns></returns>
            [HttpPatch("{categoryId}")]
            public async Task<IActionResult> UpdateCategory(Guid categoryId, JsonPatchDocument<UpdateCategoryDto> patchDocument)
            {
                var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (categoryEntity is null)
                {
                    _logger.LogError($"Category with Id {categoryId} not found when accessing {nameof(UpdateCategory)}");
                    return NotFound();
                }

                var categoryToPatch = _mapper.Map<UpdateCategoryDto>(categoryEntity);
                patchDocument.ApplyTo(categoryToPatch);

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                    _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(UpdateCategory)} with {errors}");
                    return UnprocessableEntity(ModelState);
                }
                if (!TryValidateModel(categoryToPatch))
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList());
                    _logger.LogError($"Encountered invalid inputs from user when accessing {nameof(UpdateCategory)} with {errors}");
                    return UnprocessableEntity(ModelState);
                }

                _mapper.Map(categoryToPatch, categoryEntity);
                await _unitOfWork.Save();
                return NoContent();
            }

            /// <summary>
            /// Delete the category
            /// </summary>
            /// <param name="categoryId"></param>
            /// <returns></returns>
            [HttpDelete("{categoryId}")]
            public async Task<IActionResult> DeleteCategory(Guid categoryId)
            {
                var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (categoryEntity is null)
                {
                    _logger.LogError($"Category with Id {categoryId} not found when accessing {nameof(DeleteCategory)}");
                    return NotFound();
                }
                _unitOfWork.Categories.Delete(categoryEntity);
                await _unitOfWork.Save();
                return NoContent();
            }
        }

    }

