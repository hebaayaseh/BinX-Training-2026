using FluentValidation;
using LibraryManagment.DTO.CategoryDto;

namespace LibraryManagment.Validations
{
    public class CreateCategoryValidtor : AbstractValidator<CreateCategoryRequestDto>
    {
        public CreateCategoryValidtor()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(200).WithMessage("Category name must not exceed 200 characters");

            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Category description is required")
                .MaximumLength(500).WithMessage("Category description must not exceed 500 characters");

            RuleFor(x => x.name)
                .Must(n => !n.Any(char.IsDigit))
                .WithMessage("Category name must not contain any numbers only characters");


        }
    }
}
