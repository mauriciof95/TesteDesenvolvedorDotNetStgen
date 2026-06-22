using GoodHamburger.Application.OrderContext.DTOs;
using GoodHamburger.Application.Utils;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.UnitOfWork;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Application.OrderContext;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IOrderRepository orderRepository, 
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResultDTO<OrderDTO, Order>> SearchAsync(BaseSearchParameters<Order> parameters, CancellationToken cancellationToken)
    {
        var result = await _orderRepository.GetPagedResultAsync(parameters, cancellationToken);

        var pagedResult = new PagedResultDTO<OrderDTO, Order>();
        pagedResult.TotalCount = result.TotalCount;
        pagedResult.SearchParameters = parameters;
        pagedResult.Rows = OrderDTO.ToDTO(result.Rows);

        return pagedResult;
    }

    public async Task<OrderDTO> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, cancellationToken);

        if (order is null) throw new NotFoundException();

        return new OrderDTO(order);
    }

    public async Task<long> CreateAsync(CreateOrderDTO dto, CancellationToken cancellationToken)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new BadRequestException("O Pedido precisar conter pelo menos 1 produto.");

        var productIds = dto.Items.Select(i => i.ProductId).ToArray();

        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);

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
        await _unitOfWork.CommitAsync(cancellationToken);

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

    public async Task UpdateAsync(UpdateOrderDTO dto, long id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, cancellationToken);

        if (order is null) throw new NotFoundException();

        if (dto.Items == null || !dto.Items.Any())
            throw new BadRequestException("O Pedido precisar conter pelo menos 1 produto.");

        var productIds = dto.Items.Select(i => i.ProductId).ToArray();

        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);

        var updatedOrderItems = new List<OrderItem>();

        foreach (var i in dto.Items)
        {
            var product = products.FirstOrDefault(x => x.Id == i.ProductId);

            if (product is null)
                throw new NotFoundException($"Produto {i.ProductId} não encontrado.");

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

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _orderRepository.DeleteOrderItens(oldOrderItems);
            _orderRepository.CreateOrderItems(updatedOrderItems);
            _orderRepository.Update(order);

            await _unitOfWork.CommitAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, cancellationToken);

        if (order is null) throw new NotFoundException();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var items = order.OrderItems;

            _orderRepository.DeleteOrderItens(items);
            _orderRepository.Delete(order);

            await _unitOfWork.CommitAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
