using FluentValidation;
using MagicVilla_CouponAPI.Models.DTO;

namespace MagicVilla_CouponAPI.Validation
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(modal => modal.Name).NotEmpty();
            RuleFor(modal => modal.Percent).InclusiveBetween(1, 100);
        }
    }
}
