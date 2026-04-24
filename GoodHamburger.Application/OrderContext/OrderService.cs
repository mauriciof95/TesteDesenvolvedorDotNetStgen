using GoodHamburger.Application.OrderContext.DTOs;
using GoodHamburger.Application.ProductContext.DTOs;
using GoodHamburger.Application.Utils;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Application.OrderContext;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public PagedResultDTO<OrderDTO> Search(BaseSearchParameters parameters)
    {
        var result = _orderRepository.GetPagedResult(parameters);

        var pagedResult = new PagedResultDTO<OrderDTO>();
        pagedResult.TotalCount = result.TotalCount;
        pagedResult.SearchParameters = parameters;
        pagedResult.Rows = OrderDTO.ToDTO(result.Rows);

        return pagedResult;
    }

    public OrderDTO GetById(long id)
    {
        var order = _orderRepository.GetByIdWithItems(id);

        if (order is null) throw new NotFoundException();

        return new OrderDTO(order);
    }

    public long Create(CreateOrderDTO dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new BadRequestException("O Pedido precisar conter pelo menos 1 produto.");

        var productIds = dto.Items.Select(i => i.ProductId).ToArray();

        var products = _productRepository.GetByIds(productIds);

        var orderItems = new List<OrderItem>();
        
        foreach(var i in dto.Items)
        {
            var product = products.FirstOrDefault(x => x.Id == i.ProductId);

            if(product is null)
                throw new NotFoundException($"Produto {i.ProductId} não encontrado.");

            if(orderItems.Any(x => x.ProductType == product.ProductType))
                throw new ValidationException("O Pedido não pode conter produtos do mesmo tipo.");

            var orderItem = new OrderItem
            {
                ProductName = product.Name,
                ProductType = product.ProductType,
                CurrentPrice = product.Price,
                ProductId = product.Id
            };

            orderItems.Add(orderItem);
        }

        var computedValues = ProcessDiscount(orderItems);

        var order = new Order
        {
            Subtotal = computedValues.subtotal,
            Discount = computedValues.discount,
            Total = computedValues.total,
            OrderItems = orderItems
        };

        order = _orderRepository.Create(order);
        _orderRepository.Commit();

        return order.Id;
    }

    private (decimal total, decimal subtotal, decimal discount) ProcessDiscount(List<OrderItem> orderItems)
    {
        var subtotal = orderItems.Sum(i => i.CurrentPrice);

        var hasSandwich = orderItems.Any(i => i.ProductType == ProductType.Sandwich);
        var hasFries = orderItems.Any(i => i.ProductType == ProductType.Fries);
        var hasDrink = orderItems.Any(i => i.ProductType == ProductType.Drink);

        decimal discount = 0;

        if (hasSandwich && hasFries && hasDrink)
            discount = subtotal * 0.20m;
        else if (hasSandwich && hasDrink)
            discount = subtotal * 0.15m;
        else if (hasSandwich && hasFries)
            discount = subtotal * 0.10m;

        var total = subtotal - discount;

        return (total, subtotal, discount);
    }

    public void Update(UpdateOrderDTO dto, long id)
    {
        var order = _orderRepository.GetByIdWithItems(id);

        if (order is null) throw new NotFoundException();

        if (dto.Items == null || !dto.Items.Any())
            throw new BadRequestException("O Pedido precisar conter pelo menos 1 produto.");

        var productIds = dto.Items.Select(i => i.ProductId).ToArray();

        var products = _productRepository.GetByIds(productIds);

        var updatedOrderItems = new List<OrderItem>();

        foreach (var i in dto.Items)
        {
            var product = products.FirstOrDefault(x => x.Id == i.ProductId);

            if (product is null)
                new NotFoundException($"Produto {i.ProductId} não encontrado.");

            if (updatedOrderItems.Any(x => x.ProductType == product.ProductType))
                throw new ValidationException("O Pedido não pode conter produtos do mesmo tipo.");

            var orderItem = new OrderItem
            {
                ProductName = product.Name,
                ProductType = product.ProductType,
                CurrentPrice = product.Price,
                ProductId = product.Id,
                OrderId = order.Id
            };

            updatedOrderItems.Add(orderItem);
        }

        var computedValues = ProcessDiscount(updatedOrderItems);
        var oldOrderItems = order.OrderItems;

        order.Subtotal = computedValues.subtotal;
        order.Total = computedValues.total;
        order.Discount = computedValues.discount;     

        _orderRepository.BeginTransaction();

        try
        {
            _orderRepository.DeleteOrderItens(oldOrderItems);
            _orderRepository.CreateOrderItems(updatedOrderItems);
            _orderRepository.Update(order);

            _orderRepository.Commit();
            _orderRepository.CommitTransaction();
        }
        catch (Exception ex)
        {
            _orderRepository.Rollback();
            throw ex;
        }
    }

    public void Delete(long id)
    {
        var order = _orderRepository.GetByIdWithItems(id);

        if (order is null) throw new NotFoundException();

        _orderRepository.BeginTransaction();
        try
        {
            var items = order.OrderItems;

            _orderRepository.DeleteOrderItens(items);
            _orderRepository.Delete(order);

            _orderRepository.Commit();
            _orderRepository.CommitTransaction();
        }
        catch (Exception ex)
        {
            _orderRepository.Rollback();
            throw ex;
        }
    }
}
