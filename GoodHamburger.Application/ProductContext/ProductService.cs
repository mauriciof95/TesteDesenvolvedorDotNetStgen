using GoodHamburger.Application.Extensions;
using GoodHamburger.Application.ProductContext.DTOs;
using GoodHamburger.Application.Utils;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Application.ProductContext;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public PagedResultDTO<ProductDTO> Search(BaseSearchParameters parameters)
    {
        var result = _productRepository.GetPagedResult(parameters);

        var pagedResult = new PagedResultDTO<ProductDTO>();
        pagedResult.TotalCount = result.TotalCount;
        pagedResult.SearchParameters = parameters;
        pagedResult.Rows = ProductDTO.ToDTO(result.Rows);

        return pagedResult;
    }

    public object GetAll()
    {
        var products = _productRepository.GetAll();
        return ProductDTO.ToDTO(products.ToList());
    }

    public MenuDTO[] GetMenu()
    {
        var result = _productRepository.GetAll();

        var menu = result.GroupBy(x => x.ProductType)
            .Select(x => new MenuDTO
            {
                Group = x.Key.GetDescription(),
                Items = x.Select(y => new MenuItemDTO(y)).OrderBy(y => y.Name).ToArray()
            })
            .ToArray();

        return menu;
    }


    public ProductDTO GetById(long id)
    {
        var product = _productRepository.GetById(id);

        if (product is null) throw new NotFoundException();

        return new ProductDTO(product);
    }

    public long Create(CreateProductDTO dto)
    {
        var product = new Product(
            dto.Name,
            dto.Price,
            dto.ProductType
        );

        product = _productRepository.Create(product);
        _productRepository.Commit();

        return product.Id;
    }

    public void Update(UpdateProductDTO dto, long id)
    {
        var product = _productRepository.GetById(id);

        if (product is null) throw new NotFoundException();
        
        product.Update(
            dto.Name,
            dto.Price,
            dto.ProductType
        );
 
        _productRepository.Update(product);
        _productRepository.Commit();
    }

    public void Delete(long id)
    {
        var product = _productRepository.GetById(id);

        if (product is null) throw new NotFoundException();

        _productRepository.Delete(product);
        _productRepository.Commit();
    }
}
