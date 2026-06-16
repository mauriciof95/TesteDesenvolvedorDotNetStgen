using System.Threading.Tasks;
using GoodHamburger.Application.Extensions;
using GoodHamburger.Application.ProductContext.DTOs;
using GoodHamburger.Application.Utils;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.UnitOfWork;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Application.ProductContext;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    )
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResultDTO<ProductDTO, Product>> SearchAsync(BaseSearchParameters<Product> parameters, CancellationToken cancellationToken)
    {
        parameters.OrderBy = (x => x.Name);
        var result = await _productRepository.GetPagedResultAsync(parameters, cancellationToken);

        var pagedResult = new PagedResultDTO<ProductDTO, Product>();
        pagedResult.TotalCount = result.TotalCount;
        pagedResult.SearchParameters = parameters;
        pagedResult.Rows = ProductDTO.ToDTO(result.Rows);

        return pagedResult;
    }

    public async Task<object> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(false, cancellationToken);
        return ProductDTO.ToDTO(products.ToList());
    }

    public async Task<MenuDTO[]> GetMenuAsync()
    {
        var result = await _productRepository.GetAllAsync();

        var menu = result.GroupBy(x => x.ProductType)
            .Select(x => new MenuDTO
            {
                Group = x.Key.GetDescription(),
                Items = x.Select(y => new MenuItemDTO(y)).OrderBy(y => y.Name).ToArray()
            })
            .ToArray();

        return menu;
    }


    public async Task<ProductDTO> GetByIdAsync(long id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null) throw new NotFoundException();

        return new ProductDTO(product);
    }

    public async Task<long> CreateAsync(CreateProductDTO dto)
    {
        var product = new Product(
            dto.Name,
            dto.Price,
            dto.ProductType
        );

        product = _productRepository.Create(product);
        await _unitOfWork.CommitAsync();

        return product.Id;
    }

    public async Task UpdateAsync(UpdateProductDTO dto, long id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null) throw new NotFoundException();
        
        product.Update(
            dto.Name,
            dto.Price,
            dto.ProductType
        );
 
        _productRepository.Update(product);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null) throw new NotFoundException();

        _productRepository.Delete(product);
        await _unitOfWork.CommitAsync();
    }
}
