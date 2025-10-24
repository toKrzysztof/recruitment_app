using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.Features.Contacts.Dto;
using RecruitmentApp.Shared.Api;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Features.Contacts.Api.Controllers;

public class CategoriesController : ApiControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
        return Ok(categoriesDto);
    }

    [Route("{id}/subcategories")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSubcategories(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        var subcategories = await _unitOfWork.Subcategories.GetSubcategoriesByCategoryId(id);
        var subcategoriesDto = _mapper.Map<List<SubcategoryDto>>(subcategories);
        return Ok(subcategoriesDto);
    }
}
