using FluentValidation;
using MagicVilla_CouponAPI.Models.DTO;

namespace MagicVilla_CouponAPI.Validation
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation()
        {
            RuleFor(modal => modal.Id).NotEmpty().GreaterThan(0);
            RuleFor(modal => modal.Name).NotEmpty();
            RuleFor(modal => modal.Percent).InclusiveBetween(1, 100);
        }
    }
}
