using GoodHamburger.Application.OrderContext;
using GoodHamburger.Application.OrderContext.DTOs;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Tests.Helpers;
using Moq;

namespace GoodHamburger.Tests;

public class OrderServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;

    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();

        _service = new OrderService(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object
        );
    }

    [Fact]
    public void Deve_aplicar_20_porcento_de_desconto_quando_combo_cheio()
    {
        var products = new Product[]
        {
            new Product("X Burger", 5,    ProductType.Sandwich).WithId(1),
            new Product("Batata",   2,    ProductType.Fries).WithId(4),
            new Product("Refri",    2.5m, ProductType.Drink).WithId(5),
        };

        _productRepositoryMock
            .Setup(x => x.GetByIds(It.IsAny<long[]>()))
            .Returns(products);

        Order createdOrder = null;

        _orderRepositoryMock
            .Setup(x => x.Create(It.IsAny<Order>()))
            .Callback<Order>(o => createdOrder = o)
            .Returns((Order o) => o);

        var dto = new CreateOrderDTO
        {
            Items = new List<CreateOrderItemDTO>
            {
                new() { ProductId = 1 },
                new() { ProductId = 4 },
                new() { ProductId = 5 }
            }
        };

        _service.Create(dto);

        Assert.NotNull(createdOrder);
        Assert.Equal(createdOrder.Subtotal * 0.20m, createdOrder.Discount);
    }

    [Fact]
    public void Deve_aplicar_15_porcento_desconto_quando_combo_for_Sanduiche_e_refri()
    {
        var products = new Product[]
        {
            new Product("X Burger", 5, ProductType.Sandwich).WithId(1),
            new Product("Refri", 2.5m, ProductType.Drink).WithId(5),
        };

        _productRepositoryMock
            .Setup(x => x.GetByIds(It.IsAny<long[]>()))
            .Returns(products);

        Order createdOrder = null;

        _orderRepositoryMock
            .Setup(x => x.Create(It.IsAny<Order>()))
            .Callback<Order>(o => createdOrder = o)
            .Returns((Order o) => o);

        var dto = new CreateOrderDTO
        {
            Items = new List<CreateOrderItemDTO>
            {
                new() { ProductId = 1 },
                new() { ProductId = 5 }
            }
        };

        _service.Create(dto);

        Assert.NotNull(createdOrder);
        Assert.Equal(createdOrder.Subtotal * 0.15m, createdOrder.Discount);
    }

    [Fact]
    public void Deve_aplicar_10_porcento_desconto_quando_combo_for_sanduiche_e_fritas()
    {
        var products = new Product[]
        {
            new Product("Teste", 5, ProductType.Sandwich ).WithId(1),
            new Product("Teste2", 2, ProductType.Fries).WithId(4)
        };

        _productRepositoryMock
            .Setup(x => x.GetByIds(It.IsAny<long[]>()))
            .Returns(products);

        Order createdOrder = null;

        _orderRepositoryMock
            .Setup(x => x.Create(It.IsAny<Order>()))
            .Callback<Order>(o => createdOrder = o)
            .Returns((Order o) => o);

        var dto = new CreateOrderDTO
        {
            Items = new List<CreateOrderItemDTO>
            {
                new() { ProductId = 1 },
                new() { ProductId = 4 }
            }
        };

        _service.Create(dto);

        Assert.NotNull(createdOrder);
        Assert.Equal(createdOrder.Subtotal * 0.10m, createdOrder.Discount);
    }

    [Fact]
    public void Deve_lancar_excecao_quando_tiver_tipos_duplicados_no_pedido()
    {
        var products = new Product[]
        {
            new Product("Teste",  1, ProductType.Sandwich).WithId(1),
            new Product("Teste2", 2, ProductType.Sandwich).WithId(2)
        };

        _productRepositoryMock
            .Setup(x => x.GetByIds(It.IsAny<long[]>()))
            .Returns(products);

        var dto = new CreateOrderDTO
        {
            Items = new List<CreateOrderItemDTO>
            {
                new() { ProductId = 1 },
                new() { ProductId = 2 }
            }
        };

        Assert.Throws<ValidationException>(() => _service.Create(dto));
    }

    [Fact]
    public void Deve_lancar_excecao_quando_pedido_nao_tiver_itens()
    {
        var dto = new CreateOrderDTO
        {
            Items = new List<CreateOrderItemDTO>()
        };

        Assert.Throws<BadRequestException>(() => _service.Create(dto));
    }

    [Fact]
    public void Deve_lancar_excecao_quando_produto_no_pedido_nao_existe()
    {
        _productRepositoryMock
            .Setup(x => x.GetByIds(It.IsAny<long[]>()))
            .Returns(Array.Empty<Product>);

        var dto = new CreateOrderDTO
        {
            Items = new List<CreateOrderItemDTO>
            {
                new() { ProductId = 99 }
            }
        };

        Assert.Throws<NotFoundException>(() => _service.Create(dto));
    }

	[Fact]
	public void Deve_deletar_pedido_quando_existir()
	{
		var order = new Order
		{
			Subtotal = 9.5m,
			Discount = 1.9m,
			Total = 7.6m,
			OrderItems = new List<OrderItem>
			{
				new() { ProductType = ProductType.Sandwich, CurrentPrice = 5 },
				new() { ProductType = ProductType.Drink, CurrentPrice = 2.5m },
				new() { ProductType = ProductType.Fries, CurrentPrice = 2 }
			}
		}.WithId(1);

		_orderRepositoryMock
			.Setup(x => x.GetByIdWithItems(1))
			.Returns(order);

		_service.Delete(1);

		_orderRepositoryMock.Verify(x => x.DeleteOrderItens(order.OrderItems), Times.Once);
		_orderRepositoryMock.Verify(x => x.Delete(order), Times.Once);
		_orderRepositoryMock.Verify(x => x.Commit(), Times.Once);
		_orderRepositoryMock.Verify(x => x.CommitTransaction(), Times.Once);
	}

	[Fact]
	public void Deve_lancar_excecao_quando_pedido_para_deletar_nao_existir()
	{
		_orderRepositoryMock
			.Setup(x => x.GetByIdWithItems(It.IsAny<long>()))
			.Returns((Order)null);

		Assert.Throws<NotFoundException>(() => _service.Delete(99));
	}

	[Fact]
	public void Deve_fazer_rollback_quando_ocorrer_erro_ao_deletar_pedido()
	{
		var order = new Order
		{
			OrderItems = new List<OrderItem>
			{
				new() { ProductType = ProductType.Sandwich, CurrentPrice = 5 }
			}
		}.WithId(1);

		_orderRepositoryMock
			.Setup(x => x.GetByIdWithItems(1))
			.Returns(order);

		_orderRepositoryMock
			.Setup(x => x.Delete(It.IsAny<Order>()))
			.Throws(new Exception("Erro ao deletar pedido"));

		Assert.Throws<Exception>(() => _service.Delete(1));

		_orderRepositoryMock.Verify(x => x.DeleteOrderItens(order.OrderItems), Times.Once);
		_orderRepositoryMock.Verify(x => x.Rollback(), Times.Once);
		_orderRepositoryMock.Verify(x => x.CommitTransaction(), Times.Never);
	}
}
