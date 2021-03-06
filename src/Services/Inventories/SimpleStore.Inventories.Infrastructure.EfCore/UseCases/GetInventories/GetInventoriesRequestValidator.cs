﻿using FluentValidation;

namespace SimpleStore.Inventories.Infrastructure.EfCore.UseCases.GetInventories
{
    public class GetInventoriesRequestValidator : AbstractValidator<GetInventoriesRequest>
    {
        public GetInventoriesRequestValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0);
            RuleFor(x => x.PageSize)
                .GreaterThan(0);
        }
    }
}
