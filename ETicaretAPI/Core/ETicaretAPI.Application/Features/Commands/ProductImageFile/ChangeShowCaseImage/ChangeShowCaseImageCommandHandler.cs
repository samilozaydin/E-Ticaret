using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage
{
    public class ChangeShowCaseImageCommandHandler : IRequestHandler<ChangeShowCaseImageCommandRequest, ChangeShowCaseImageCommandResponse>
    {
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;


        public ChangeShowCaseImageCommandHandler(
            IProductImageFileReadRepository productImageFileReadRepository, 
            IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowCaseImageCommandResponse> Handle(ChangeShowCaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileReadRepository.Table
                .Include(p => p.Products)
                .SelectMany(p => p.Products, (pif, p) => new
                {
                    pif = pif,
                    p= p
                });

            var product = await query.FirstOrDefaultAsync(p => Guid.Parse(request.ProductId) == p.p.Id && p.pif.ShowCase);
            if(product != null)
                product.pif.ShowCase = false;

            var data = await query.FirstOrDefaultAsync(p => Guid.Parse(request.ImageId) == p.pif.Id);
            if(data != null)
                data.pif.ShowCase = true;

            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
