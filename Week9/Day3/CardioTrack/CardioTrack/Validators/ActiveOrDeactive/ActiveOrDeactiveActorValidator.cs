using CardioTrack.DTOs.Admin;
using FluentValidation;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CardioTrack.Validators.ActiveOrDeactive
{
    public class ActiveOrDeactiveActorValidator : AbstractValidator<ActiveDeactiveDto>
    {
        public ActiveOrDeactiveActorValidator()
        {
            RuleFor(x => x.ActorId).GreaterThan(0);
        }
    }
}
