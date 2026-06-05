using System;
using GoodHamburger.Application.ProductContext;
using GoodHamburger.Application.ProductContext.DTOs;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Tests.Helpers;
using Moq;

namespace GoodHamburger.Tests;

public class ProductServiceTests
{
	private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _service = new ProductService(_productRepositoryMock.Object);
    }

	[Fact]
	public void Deve_retornar_todos_os_produtos()
	{
		var products = new Product[]
		{
			new Product("X Burger", 5, ProductType.Sandwich).WithId(1),
			new Product("Batata", 2, ProductType.Fries).WithId(2)
		};

		_productRepositoryMock
			.Setup(x => x.GetAll(false))
			.Returns(products);

		var result = _service.GetAll();

		var list = Assert.IsType<ProductDTO[]>(result);
		Assert.Equal(2, list.Count());
	}

	[Fact]
	public void Deve_retornar_todos_os_produtos_por_tipo_para_o_cardapio()
	{
		var products = new Product[]
		{
			new Product("X Burger", 1, ProductType.Sandwich).WithId(1),
			new Product("X Egg",    1, ProductType.Sandwich).WithId(2),
			new Product("Batata",   1, ProductType.Fries).WithId(3)
		};

		_productRepositoryMock
			.Setup(x => x.GetAll(false))
			.Returns(products);

		var result = _service.GetMenu();

		Assert.Equal(2, result.Length);
		Assert.Contains(result, x => x.Items.Length == 2);
		Assert.Contains(result, x => x.Items.Length == 1);
	}

	[Fact]
	public void Deve_retornar_o_produto_por_id()
	{
		var product = new Product("X Burger", 5, ProductType.Sandwich).WithId(1);

		_productRepositoryMock
			.Setup(x => x.GetById(1))
			.Returns(product);

		var result = _service.GetById(1);

		Assert.Equal(product.Name, result.Name);
	}

	[Fact]
	public void Deve_lancar_excecao_quando_produto_nao_existir()
	{
		_productRepositoryMock
			.Setup(x => x.GetById(It.IsAny<long>()))
			.Returns((Product)null);

		Assert.Throws<NotFoundException>(() => _service.GetById(1));
	}

	[Fact]
	public void Deve_criar_o_produto()
	{
		Product createdProduct = null;

		_productRepositoryMock
			.Setup(x => x.Create(It.IsAny<Product>()))
			.Callback<Product>(p => 
			{
				p.WithId(10);
				createdProduct = p;
			})
			.Returns((Product p) => p);

		var dto = new CreateProductDTO
		{
			Name = "X Bacon",
			Price = 7,
			ProductType = ProductType.Sandwich
		};

		var result = _service.Create(dto);

		Assert.Equal(10, result);
		Assert.NotNull(createdProduct);
		Assert.Equal(dto.Name, createdProduct.Name);

		_productRepositoryMock.Verify(x => x.Commit(), Times.Once);
	}

	[Fact]
	public void Deve_atualizar_o_produto_quando_existir()
	{
		var product = new Product("Old", 1, ProductType.Drink).WithId(1);

		_productRepositoryMock
			.Setup(x => x.GetById(1))
			.Returns(product);

		var dto = new UpdateProductDTO
		{
			Name = "New",
			Price = 10,
			ProductType = ProductType.Sandwich
		};

		_service.Update(dto, 1);

		Assert.Equal("New", product.Name);
		Assert.Equal(10, product.Price);

		_productRepositoryMock.Verify(x => x.Update(product), Times.Once);
		_productRepositoryMock.Verify(x => x.Commit(), Times.Once);
	}

	[Fact]
	public void Deve_lancar_excecao_quando_produto_para_atualizar_nao_existir()
	{
		_productRepositoryMock
			.Setup(x => x.GetById(It.IsAny<long>()))
			.Returns((Product) null);

		var dto = new UpdateProductDTO();

		Assert.Throws<NotFoundException>(() => _service.Update(dto, 1));
	}

	[Fact]
	public void Deve_deletar_o_produto_quando_existir()
	{
		var product = new Product("Teste", 1, ProductType.Sandwich).WithId(1);

		_productRepositoryMock
			.Setup(x => x.GetById(1))
			.Returns(product);

		_service.Delete(1);

		_productRepositoryMock.Verify(x => x.Delete(product), Times.Once);
		_productRepositoryMock.Verify(x => x.Commit(), Times.Once);
	}

	[Fact]
	public void Deve_lancar_excecao_quando_produto_para_deletar_nao_existir()
	{
		_productRepositoryMock
			.Setup(x => x.GetById(It.IsAny<long>()))
			.Returns((Product) null);

		Assert.Throws<NotFoundException>(() => _service.Delete(1));
	}
}
