using FluentValidation;
using LibraryManagment.DTO.CategoryDto;

namespace LibraryManagment.Validations
{
    public class UpdateCategoryValidtor :AbstractValidator<UpdateCategoryRequestDto>
    {
        public UpdateCategoryValidtor()
        {
            
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Category id is required");

            RuleFor(x => x.name)
                .MaximumLength(200).WithMessage("Category name must not exceed 200 characters");

            RuleFor(x => x.name)
               .MaximumLength(500).WithMessage("Category description must not exceed 500 characters");

        }
    }
}
