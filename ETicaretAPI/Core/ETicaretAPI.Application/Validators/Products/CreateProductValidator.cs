using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please enter your name")
                .MinimumLength(2)
                .MaximumLength(50)
                    .WithMessage("Your name should be minimum 2 length and maximum 50 length ");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please enter your stock")
                .Must(s => s >= 0)
                    .WithMessage("Your stock cannot be less than 0");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please enter your price")
                .Must(s => s >= 0)
                    .WithMessage("Your price cannot be less than 0");
        }
    }
}
